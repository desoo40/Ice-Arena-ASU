DROP TABLE IF EXISTS operation;
DROP TABLE IF EXISTS _transaction;

CREATE TABLE operation(
    id INTEGER PRIMARY KEY,
    name TEXT NOT NULL
);

CREATE TABLE _transaction(
    id INTEGER PRIMARY KEY,
    transaction_date DATETIME NOT NULL,
    operation_id INTEGER NOT NULL,
    name TEXT NOT NULL,
    amount INTEGER NOT NULL
);