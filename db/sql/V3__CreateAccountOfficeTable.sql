CREATE TABLE IF NOT EXISTS Account_Office (
    account_id INT,
    office_id INT,
    account_rank INT,
    FOREIGN KEY (account_id) REFERENCES Account(id),
    FOREIGN KEY (office_id) REFERENCES Office(id)
);
