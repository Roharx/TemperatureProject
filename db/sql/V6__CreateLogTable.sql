CREATE TABLE IF NOT EXISTS Log_Table (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    office_id INT,
    room_id INT,
    action VARCHAR(255),
    FOREIGN KEY (account_id) REFERENCES Account(id),
    FOREIGN KEY (office_id) REFERENCES Office(id),
    FOREIGN KEY (room_id) REFERENCES Room(id)
);
