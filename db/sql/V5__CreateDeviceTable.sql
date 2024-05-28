CREATE TABLE IF NOT EXISTS Device (
    id INT AUTO_INCREMENT PRIMARY KEY,
    office_id INT,
    room_id INT,
    FOREIGN KEY (office_id) REFERENCES Office(id),
    FOREIGN KEY (room_id) REFERENCES Room(id)
);
