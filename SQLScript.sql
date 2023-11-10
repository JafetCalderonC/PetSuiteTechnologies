/* 
    This script is used to create the database and tables for the project.
*/

CREATE TABLE admin_settings (
	setting_id INT PRIMARY KEY IDENTITY(1,1),
    setting_name NVARCHAR(50),
    value DECIMAL(10, 2),
    created_date DATETIME,
    modified_date DATETIME
);

CREATE TABLE users (
    user_id INT PRIMARY KEY IDENTITY(1,1),
    is_otp_verified BIT,
    password_hash NVARCHAR(128),
    role NVARCHAR(20),
    status TINYINT,
    first_name NVARCHAR(50),
    last_name NVARCHAR(50),
    identification_type NVARCHAR(30),
    identifier_value NVARCHAR(60),
    email NVARCHAR(50),
    profile_photo_url NVARCHAR(MAX),    
    theme_preference NVARCHAR(20),
    created_date DATETIME,
    modified_date DATETIME,    
    address_latitude FLOAT,
    address_longitude FLOAT
);

CREATE TABLE password_histories (
    pass_history INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    password_hash NVARCHAR(MAX),

    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE phones (
    phone_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT,
    phone_number NVARCHAR(20),

    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE discount_codes (
    discount_code_id INT PRIMARY KEY IDENTITY(1,1),
    code NVARCHAR(30) UNIQUE,
    description NVARCHAR(Max),
    status TINYINT,
    total_issued INT,
    applied_count INT,
    discount DECIMAL(10, 2),
    created_date DATETIME,
    modified_date DATETIME
);

CREATE TABLE services (
    service_id INT PRIMARY KEY IDENTITY(1,1),
    service_name NVARCHAR(50),
    description NVARCHAR(Max),
    status TINYINT,
    cost DECIMAL(10, 2),
    created_date DATETIME,
    modified_date DATETIME
);

CREATE TABLE packages (
    package_id INT PRIMARY KEY IDENTITY(1,1),
    package_name NVARCHAR(50),
    description NVARCHAR(200),
    status TINYINT,
    room_id int,
    pet_breed_type NVARCHAR(50),
    pet_size NVARCHAR(20),
    pet_aggressiveness TINYINT,
    created_date DATETIME,
    modified_date DATETIME
    
    FOREIGN KEY (room_id) REFERENCES rooms(room_id)
);

CREATE TABLE package_services (
    package_id INT,
    service_id INT,
    PRIMARY KEY (package_id, service_id),

    FOREIGN KEY (package_id) REFERENCES packages(package_id),
    FOREIGN KEY (service_id) REFERENCES services(service_id)
);

CREATE TABLE pets (
    pet_id INT PRIMARY KEY IDENTITY(1,1),
    pet_name NVARCHAR(50),
    status TINYINT,
    description NVARCHAR(Max),
    age TINYINT,
    breed NVARCHAR(50),
    aggressiveness TINYINT,
    created_date DATETIME,
    modified_date DATETIME,

    FOREIGN KEY (user_id) REFERENCES users(user_id)
);

CREATE TABLE users_pets (
    user_id INT,
    pet_id INT,

    PRIMARY KEY (user_id, pet_id),
    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (pet_id) REFERENCES pets(pet_id)
);

CREATE TABLE pet_photo (
    pet_photo_id INT PRIMARY KEY IDENTITY(1,1),
    pet_id INT,
    photo_url NVARCHAR(MAX),
    FOREIGN KEY (pet_id) REFERENCES pets(pet_id)
);

CREATE TABLE reservations (
    reservation_id INT PRIMARY KEY IDENTITY(1,1),
    start_date DATE,
    end_date DATE,
    user_id INT,
    pet_id INT,
	package_id INT,
    created_date DATETIME,
    modified_date DATETIME,

    FOREIGN KEY (user_id) REFERENCES users(user_id),
    FOREIGN KEY (pet_id) REFERENCES pets(pet_id),
	FOREIGN KEY (package_id) REFERENCES packages(package_id)
);

CREATE TABLE invoices (
    invoice_id INT PRIMARY KEY IDENTITY(1,1),
    invoice_number NVARCHAR(30),
    issue_date DATE,
    due_date DATE,
    user_id INT,
	reservation_id INT,
    total_amount DECIMAL(10, 2),
    status TINYINT,
    tax_amount DECIMAL(10, 2),
    
    discount_code NVARCHAR(30),
    discount_amount DECIMAL(10, 2),

    FOREIGN KEY (user_id) REFERENCES users(user_id),
	FOREIGN KEY (reservation_id) REFERENCES reservations(reservation_id)
);

CREATE TABLE invoice_details (
    invoice_detail_id INT PRIMARY KEY IDENTITY(1,1),
    invoice_id INT,
    service_id INT,
    package_id INT,
    room_id INT,
    reservation_id INT,
    price DECIMAL(10, 2),

    FOREIGN KEY (invoice_id) REFERENCES invoices(invoice_id),
    FOREIGN KEY (service_id) REFERENCES services(service_id),
    FOREIGN KEY (package_id) REFERENCES packages(package_id),
    FOREIGN KEY (room_id) REFERENCES rooms(room_id),
    FOREIGN KEY (reservation_id) REFERENCES reservations(reservation_id)
);

CREATE TABLE unwanted_services (
    reservation_id INT,
    service_id INT,
    PRIMARY KEY (reservation_id, service_id),

    FOREIGN KEY (reservation_id) REFERENCES reservations(reservation_id),
    FOREIGN KEY (service_id) REFERENCES services(service_id)
);

CREATE TABLE iot_room_records (
    iot_room_record_id INT PRIMARY KEY IDENTITY(1,1),
    iot_id INT,
    room_id INT,
    temperature DECIMAL(10, 2),
    humidity DECIMAL(10, 2),
    light DECIMAL(10, 2),
    created_date DATETIME,

    FOREIGN KEY (room_id) REFERENCES rooms(room_id)
);

CREATE TABLE iot_pet_records (
    iot_pet_record_id INT PRIMARY KEY IDENTITY(1,1),
    iot_id INT,
    pet_id INT,
    pulse_rate INT,
    created_date DATETIME,

    FOREIGN KEY (pet_id) REFERENCES pets(pet_id)
);

CREATE TABLE rooms (
	room_id INT PRIMARY KEY IDENTITY(1,1),
	room_name NVARCHAR(50),
	description NVARCHAR(Max),
	status TINYINT,
	cost DECIMAL(10, 2),
	created_date DATETIME,
	modified_date DATETIME
);

CREATE TABLE iot (
	iot_id INT PRIMARY KEY,
    iot_type NVARCHAR(50),
	status TINYINT,
	reservation_id INT NULL,
	created_date DATETIME,
    modified_date DATETIME,
);