CREATE TABLE IF NOT EXISTS User_Preferences (
    id INT AUTO_INCREMENT PRIMARY KEY,
    account_id INT,
    room_id INT,
    desired_temp DOUBLE PRECISION,
    FOREIGN KEY (account_id) REFERENCES Account(id),
    FOREIGN KEY (room_id) REFERENCES Room(id)
);
