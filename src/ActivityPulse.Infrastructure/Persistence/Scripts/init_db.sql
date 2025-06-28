CREATE TABLE IF NOT EXISTS Session (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NULL,
    ProcessName TEXT NOT NULL,
    ProcessPath TEXT NOT NULL,
    WindowTitle TEXT,
    CreatedAt DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS UserState (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    State INTEGER NOT NULL,               -- enum int: 0-Active, 1-Idle, 2-Away
    StartTime DATETIME NOT NULL,
    EndTime DATETIME NULL,
    CreatedAt DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS SessionAggregate (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date DATE NOT NULL,
    ProcessName TEXT NOT NULL,
    ProcessPath TEXT NOT NULL,
    TotalDuration REAL NOT NULL,      
    SessionCount INTEGER NOT NULL,
    CreatedAt DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS UserStateAggregate (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date DATE NOT NULL,
    State TEXT NOT NULL,
    Duration REAL NOT NULL,
    CreatedAt DATETIME NOT NULL
);

CREATE TABLE IF NOT EXISTS AppMetaData (
    ProcessName TEXT PRIMARY KEY,
    DisplayName TEXT,
    IconPath TEXT,
    CreatedAt DATETIME NOT NULL
);

CREATE INDEX IF NOT EXISTS idx_Session_StartTime ON Session(StartTime);
CREATE INDEX IF NOT EXISTS idx_Session_ProcessName ON Session(ProcessName);
CREATE INDEX IF NOT EXISTS idx_UserState_StartTime ON UserState(StartTime);
CREATE INDEX IF NOT EXISTS idx_SessionAggregate_Date ON SessionAggregate(Date);
