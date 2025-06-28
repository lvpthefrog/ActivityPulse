using ActivityPulse.Domain;
using Microsoft.Extensions.Logging;
using static ActivityPulse.Domain.Enums;

namespace ActivityPulse.Application
{
    public class TrackingService : ITrackingService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserStateRepository _userStateRepository;
        private readonly ISystemProvider _win32Provider;
        private readonly ILogger<TrackingService> _logger;

        private ActiveWindowInfo? _lastWindow;
        private Session? _currentSession;
        private UserState? _currentState;
        private readonly TimeSpan _idleThreshold = TimeSpan.FromMinutes(15);

        public TrackingService(
            ISessionRepository sessionRepository,
            IUserStateRepository userStateRepository,
            ISystemProvider win32Provider,
            ILogger<TrackingService> logger
            )
        {
            _sessionRepository = sessionRepository;
            _userStateRepository = userStateRepository;
            _win32Provider = win32Provider;
            _logger = logger;
        }

        public async Task TrackAsync()
        {
            try
            {
                var now = DateTime.Now;
                await TrackSession(now);
                await TrackUserState(now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
            }
        }

        private async Task TrackSession(DateTime now)
        {
            var window = _win32Provider.GetActiveWindowInfo();

            bool changed = _lastWindow == null
                || window.ProcessName != _lastWindow.ProcessName
                || window.ProcessPath != _lastWindow.ProcessPath;

            if (changed && !string.IsNullOrEmpty(window.ProcessName))
            {
                if (_currentSession != null)
                {
                    _currentSession.EndTime = DateTime.Now;
                    await _sessionRepository.UpdateAsync(_currentSession);
                }
                var newSession = CreateSession(window, now);
                var newSessionId = await _sessionRepository.AddAsync(newSession);
                newSession.Id = newSessionId;
                _currentSession = newSession;
                _lastWindow = window;
            }
            else if (_currentSession != null)
            {
                _currentSession.EndTime = now;
                await _sessionRepository.UpdateAsync(_currentSession);
            }
            else
            {
                _currentSession = CreateSession(window, now);
                var newSessionId = await _sessionRepository.AddAsync(_currentSession);
                _currentSession.Id = newSessionId;
                _lastWindow = window;
            }
        }

        private async Task TrackUserState(DateTime now)
        {
            var idleTime = _win32Provider.GetIdleTime();
            var newSate = idleTime > _idleThreshold
                ? State.Idle
                : State.Active;
            bool changed = _currentState == null || _currentState.State != newSate;
            if (changed)
            {
                if (_currentState != null)
                {
                    _currentState.EndTime = now;
                    await _userStateRepository.UpdateAsync(_currentState);
                }
                var newUserState = CreateUserState(newSate, now);
                var newUserStateId = await _userStateRepository.AddAsync(newUserState);
                newUserState.Id = newUserStateId;
                _currentState = newUserState;
            }
            else if (_currentState != null)
            {
                _currentState.EndTime = now;
                await _userStateRepository.UpdateAsync(_currentState);
            }
            else
            {
                _currentState = CreateUserState(newSate, now);
                var newUserStateId = await _userStateRepository.AddAsync(_currentState);
                _currentState.Id = newUserStateId;
            }

        }

        private UserState CreateUserState(State state, DateTime now)
        {
            return new UserState
            {
                State = state,
                StartTime = now,
                EndTime = now,
                CreatedAt = now
            };
        }

        private Session CreateSession(ActiveWindowInfo window, DateTime now)
        {
            return new Session
            {
                StartTime = now,
                EndTime = now,
                ProcessName = window.ProcessName,
                ProcessPath = window.ProcessPath,
                WindowTitle = window.WindowTitle,
                CreatedAt = now
            };
        }
    }
}
