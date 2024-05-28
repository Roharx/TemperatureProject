CREATE TABLE IF NOT EXISTS Room (
    id INT AUTO_INCREMENT PRIMARY KEY,
    office_id INT,
    name VARCHAR(255),
    physical_overlay_enabled BOOLEAN DEFAULT TRUE,
    desired_temp REAL,
    window_toggle BOOLEAN DEFAULT TRUE,
    req_rank INT,
    FOREIGN KEY (office_id) REFERENCES Office(id)
);
