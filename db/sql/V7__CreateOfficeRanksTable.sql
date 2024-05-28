CREATE TABLE IF NOT EXISTS Office_Ranks (
    id INT AUTO_INCREMENT PRIMARY KEY,
    office_id INT,
    rank_id INT,
    name VARCHAR(255),
    FOREIGN KEY (office_id) REFERENCES Office(id)
);
