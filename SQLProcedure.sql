/* 
	This script is used to create the stored procedures for the project.
*/

/*
    User stored procedures 
*/

-- Create user
CREATE PROCEDURE CREATE_USER_PR
    @P_IS_PASSWORD_REQUIRED_CHANGE BIT,
    @P_PASSWORD_HASH NVARCHAR(MAX),
    @P_PASSWORD_SALT NVARCHAR(MAX),
    @P_ROLE NVARCHAR(20),
    @P_STATUS TINYINT,
    @P_FIRST_NAME NVARCHAR(50),
    @P_LAST_NAME NVARCHAR(50),
    @P_IDENTIFICATION_TYPE NVARCHAR(30),
    @P_IDENTIFIER_VALUE NVARCHAR(60),
    @P_EMAIL NVARCHAR(50),
    @P_CLOUNDINARY_PUBLIC_ID NVARCHAR(MAX),
    @P_THEME_PREFERENCE NVARCHAR(20),
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_ADDRESS_LATITUDE FLOAT,
    @P_ADDRESS_LONGITUDE FLOAT,
    @P_ID INT OUTPUT
AS
BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO users(
        is_password_required_change, 
		password_hash,
        password_salt,
        role, 
        status, 
        first_name, 
        last_name, 
        identification_type, 
        identifier_value, 
        email, 
        cloudinary_public_id, 
        theme_preference, 
        created_date, 
        modified_date, 
        address_latitude, 
        address_longitude)
    OUTPUT INSERTED.user_id INTO @ID
    VALUES (
        @P_IS_PASSWORD_REQUIRED_CHANGE, 
        @P_PASSWORD_HASH, 
        @P_PASSWORD_SALT,
        @P_ROLE, 
        @P_STATUS, 
        @P_FIRST_NAME, 
        @P_LAST_NAME, 
        @P_IDENTIFICATION_TYPE, 
        @P_IDENTIFIER_VALUE, 
        @P_EMAIL, 
        @P_CLOUNDINARY_PUBLIC_ID, 
        @P_THEME_PREFERENCE, 
        @P_CREATED_DATE, 
        @P_MODIFIED_DATE, 
        @P_ADDRESS_LATITUDE, 
        @P_ADDRESS_LONGITUDE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Add phone
CREATE PROCEDURE ADD_PHONE_NUMBER_TO_USER_PR
    @P_USER_ID INT,
    @P_PHONE_NUMBER NVARCHAR(20),

    @P_ID INT OUTPUT
AS
BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO phones(user_id, phone_number)
    OUTPUT INSERTED.phone_id INTO @ID
    VALUES (@P_USER_ID, @P_PHONE_NUMBER)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Remove phone
CREATE PROCEDURE REMOVE_PHONE_NUMBER_TO_USER_PR
    @P_USER_ID INT,
    @P_PHONE_ID INT
AS
BEGIN
    DELETE FROM phones
    WHERE user_id = @P_USER_ID AND phone_id = @P_PHONE_ID
END
GO

-- Retrive phono by phone number
CREATE PROCEDURE RETRIEVE_USER_BY_PHONE_NUMBER_PR
	@P_PHONE_NUMBER NVARCHAR(20)
AS
BEGIN
	SELECT TOP 1 
		us.user_id, 
		us.is_password_required_change, 
		us.password_hash,
        us.password_salt,
		us.role, 
		us.status, 
		us.first_name, 
		us.last_name, 
		us.identification_type, 
		us.identifier_value, 
		us.email, 
		us.cloudinary_public_id, 
		us.theme_preference, 
		us.created_date, 
		us.modified_date, 
		us.address_latitude, 
		us.address_longitude
	FROM phones
    INNER JOIN users AS us ON phones.user_id = us.user_id
    WHERE phones.phone_number = @P_PHONE_NUMBER AND status != 0
END
GO

-- Retrieve phones by user id
CREATE PROCEDURE RETRIEVE_PHONE_NUMBERS_BY_USER_ID_PR
    @P_USER_ID INT
AS
BEGIN
    SELECT phone_number
    FROM phones
    WHERE user_id = @P_USER_ID
END
GO

-- Update user
CREATE PROCEDURE UPDATE_USER_PR
    @P_USER_ID INT,   
    @P_PASSWORD_HASH NVARCHAR(MAX),
    @P_PASSWORD_SALT NVARCHAR(MAX),
    @P_ROLE NVARCHAR(20),
    @P_STATUS TINYINT,
    @P_FIRST_NAME NVARCHAR(50),
    @P_LAST_NAME NVARCHAR(50),
    @P_IDENTIFICATION_TYPE NVARCHAR(30),
    @P_IDENTIFIER_VALUE NVARCHAR(60),
    @P_EMAIL NVARCHAR(50),
    @P_CLOUNDINARY_PUBLIC_ID NVARCHAR(MAX),
    @P_THEME_PREFERENCE NVARCHAR(20),  
    @P_MODIFIED_DATE DATETIME,
    @P_ADDRESS_LATITUDE FLOAT,
    @P_ADDRESS_LONGITUDE FLOAT,
    @P_IS_PASSWORD_REQUIRED_CHANGE BIT
AS
BEGIN
    UPDATE users
    SET role = @P_ROLE,
        password_hash = @P_PASSWORD_HASH,
        password_salt = @P_PASSWORD_SALT,
        is_password_required_change = @P_IS_PASSWORD_REQUIRED_CHANGE,
        status = @P_STATUS,
        first_name = @P_FIRST_NAME,
        last_name = @P_LAST_NAME,
        identification_type = @P_IDENTIFICATION_TYPE,
        identifier_value = @P_IDENTIFIER_VALUE,
        email = @P_EMAIL,
        cloudinary_public_id = @P_CLOUNDINARY_PUBLIC_ID,
        theme_preference = @P_THEME_PREFERENCE,
        modified_date = @P_MODIFIED_DATE,
        address_latitude = @P_ADDRESS_LATITUDE,
        address_longitude = @P_ADDRESS_LONGITUDE
    WHERE user_id = @P_USER_ID AND status != 0
END
GO

-- Delete user
CREATE PROCEDURE DELETE_USER_PR
    @P_USER_ID INT,
    @P_MODIFIED_DATE DATETIME
AS
BEGIN
    UPDATE users
    SET status = 0,
        modified_date = @P_MODIFIED_DATE
    WHERE user_id = @P_USER_ID
END
GO

-- Retrieve user by id
CREATE PROCEDURE RETRIEVE_USER_BY_ID_PR
    @P_USER_ID INT
AS
BEGIN
    SELECT TOP 1 
		user_id, 
		is_password_required_change,
        password_hash,
        password_salt,
		role, 
		status, 
		first_name, 
		last_name, 
		identification_type, 
		identifier_value, 
		email, 
		cloudinary_public_id, 
		theme_preference, 
		created_date, 
		modified_date, 
		address_latitude, 
		address_longitude
    FROM users
    WHERE user_id = @P_USER_ID AND status != 0
END
GO

-- Retrieve user by email
CREATE PROCEDURE RETRIEVE_USER_BY_EMAIL_PR
    @P_EMAIL NVARCHAR(50)
AS
BEGIN
    SELECT TOP 1 
		user_id, 
		is_password_required_change, 
		password_hash,
        password_salt,
		role, 
		status, 
		first_name, 
		last_name, 
		identification_type, 
		identifier_value, 
		email, 
		cloudinary_public_id, 
		theme_preference, 
		created_date, 
		modified_date, 
		address_latitude, 
		address_longitude
    FROM users
    WHERE email = @P_EMAIL AND status != 0
END
GO

-- Retrieve all users
CREATE PROCEDURE RETRIEVE_ALL_USERS_PR AS
BEGIN
    SELECT 
		user_id, 
		is_password_required_change,
        password_hash,
        password_salt,
		role, 
		status, 
		first_name, 
		last_name, 
		identification_type, 
		identifier_value, 
		email, 
		cloudinary_public_id, 
		theme_preference, 
		created_date, 
		modified_date, 
		address_latitude, 
		address_longitude
    FROM users
    WHERE status != 0
END
GO

-- Retrive all clients
CREATE PROCEDURE RETRIEVE_ALL_CLIENTS_PR AS BEGIN
	SELECT 
		user_id, 
		is_password_required_change, 
		password_hash,
        password_salt,
		role, 
		status, 
		first_name, 
		last_name, 
		identification_type, 
		identifier_value, 
		email, 
		cloudinary_public_id, 
		theme_preference, 
		created_date, 
		modified_date, 
		address_latitude, 
		address_longitude
	FROM users
	WHERE status != 0 AND role = 1
END
GO

/* 
    Pet stored procedures
*/

-- Create pet
CREATE PROCEDURE CREATE_PET_PR
    @P_PET_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_AGE TINYINT,
    @P_BREED NVARCHAR(50),
    @P_AGGRESSIVENESS TINYINT,
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_USER_ID INT,
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO pets(
		pet_name, 
		status, 
		description, 
		age, 
		breed, 
		aggressiveness, 
		created_date, 
		modified_date)
    OUTPUT INSERTED.pet_id INTO @ID
    VALUES (
		@P_PET_NAME, 
		@P_STATUS, 
		@P_DESCRIPTION, 
		@P_AGE, 
		@P_BREED, 
		@P_AGGRESSIVENESS, 
		@P_CREATED_DATE, 
		@P_MODIFIED_DATE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Add pet pic
CREATE PROCEDURE ADD_PHOTOS_TO_PET_PR
    @P_PET_ID INT,
    @P_CLOUNDINARY_PUBLIC_ID NVARCHAR(MAX),
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO pet_pics(pet_id, cloudinary_public_id)
    OUTPUT INSERTED.pet_photo_id INTO @ID
    VALUES (@P_PET_ID, @P_CLOUNDINARY_PUBLIC_ID)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Remove pet pic
CREATE PROCEDURE REMOVE_PET_PHOTO_PR
    @P_PET_ID INT,
    @P_PIC_ID INT
AS BEGIN
    DELETE FROM pet_pics
    WHERE pet_id = @P_PET_ID AND pic_id = @P_PIC_ID
END
GO

-- Retrieve pet photos by pet id
CREATE PROCEDURE RETRIEVE_PHOTOS_BY_PET_ID_PR
    @P_PET_ID INT
AS BEGIN
    SELECT pic_id, pet_id, cloudinary_public_id
    FROM pet_pics
    WHERE pet_id = @P_PET_ID
END
GO

-- Update pet
CREATE PROCEDURE UPDATE_PET_PR
    @P_PET_ID INT,
    @P_PET_NAME NVARCHAR(50),
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_STATUS TINYINT,
    @P_AGE TINYINT,
    @P_BREED NVARCHAR(50),
    @P_AGGRESSIVENESS TINYINT,
    @P_MODIFIED_DATE DATETIME
AS BEGIN
    UPDATE pets
    SET pet_name = @P_PET_NAME,
        description = @P_DESCRIPTION,
        age = @P_AGE,
        breed = @P_BREED,
        aggressiveness = @P_AGGRESSIVENESS,
        modified_date = @P_MODIFIED_DATE
    WHERE pet_id = @P_PET_ID AND status != 0
END
GO

-- Delete pet
CREATE PROCEDURE DELETE_PET_PR
    @P_PET_ID INT
AS BEGIN
    UPDATE pets
    SET status = 0
    WHERE pet_id = @P_PET_ID
END
GO

-- Retrieve pet by id
CREATE PROCEDURE RETRIEVE_PET_BY_ID_PR
    @P_PET_ID INT
AS BEGIN
    SELECT TOP 1 
		pet_id, 
		pet_name, 
		status, 
		description, 
		age, 
		breed, 
		aggressiveness, 
		created_date, 
		modified_date
    FROM pets
    WHERE pet_id = @P_PET_ID AND status != 0
END
GO

-- Retrieve all pets
CREATE PROCEDURE RETRIEVE_ALL_PETS_PR AS BEGIN
    SELECT 
		pet_id, 
		pet_name, 
		status, 
		description, 
		age, 
		breed, 
		aggressiveness,		
		created_date, 
		modified_date
    FROM pets 
    WHERE status != 0
END
GO

-- Retrieve pets by user id
CREATE PROCEDURE RETRIEVE_PETS_BY_USER_ID_PR
    @P_USER_ID INT
AS BEGIN
    SELECT 
        p.pet_id, 
        p.pet_name, 
        p.status, 
        p.description, 
        p.age, 
        p.breed,
        p.aggressiveness, 
        p.created_date, 
        p.modified_date
    FROM pets p
    INNER JOIN users_pets up ON p.pet_id = up.pet_id
    WHERE up.user_id = @P_USER_ID AND p.status != 0
END
GO

/*
	Service stored procedures
*/

-- Create service
CREATE PROCEDURE CREATE_SERVICE_PR
    @P_SERVICE_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_COST DECIMAL(10, 2),
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO services(service_name, status, description, cost, created_date, modified_date)
    OUTPUT INSERTED.service_id INTO @ID
    VALUES (@P_SERVICE_NAME, @P_STATUS, @P_DESCRIPTION, @P_COST, @P_CREATED_DATE, @P_MODIFIED_DATE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Update service
CREATE PROCEDURE UPDATE_SERVICE_PR
    @P_SERVICE_ID INT,
    @P_SERVICE_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_COST DECIMAL(10, 2),
    @P_MODIFIED_DATE DATETIME
AS BEGIN
    UPDATE services
    SET service_name = @P_SERVICE_NAME,
        status = @P_STATUS,
        description = @P_DESCRIPTION,
        cost = @P_COST,
        modified_date = @P_MODIFIED_DATE
    WHERE service_id = @P_SERVICE_ID AND status != 0
END
GO

-- Delete service
CREATE PROCEDURE DELETE_SERVICE_PR
    @P_SERVICE_ID INT
AS BEGIN
    UPDATE services
    SET status = 0
    WHERE service_id = @P_SERVICE_ID
END
GO

-- Retrieve service by id
CREATE PROCEDURE RETRIEVE_SERVICE_BY_ID_PR
    @P_SERVICE_ID INT
AS BEGIN
    SELECT 
        service_id, 
        service_name, 
        status, 
        description, 
        cost, 
        created_date, 
        modified_date
    FROM services
    WHERE service_id = @P_SERVICE_ID AND status != 0
END
GO

-- Retrieve all services
CREATE PROCEDURE RETRIEVE_ALL_SERVICES_PR AS BEGIN
    SELECT 
        service_id, 
        service_name, 
        status, 
        description, 
        cost, 
        created_date, 
        modified_date
    FROM services
    WHERE status != 0
END
GO

-- Retrieve available services
CREATE PROCEDURE RETRIEVE_AVAILABLE_SERVICES_PR AS BEGIN
    SELECT 
        service_id, 
        service_name, 
        status, 
        description, 
        cost, 
        created_date, 
        modified_date
    FROM services
    WHERE status = 1
END
GO


/*
	Package stored procedures
*/

-- Create package
CREATE PROCEDURE CREATE_PACKAGE_PR
    @P_PACKAGE_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(200),
    @P_ROOM_ID INT,
    @P_PET_BREED_TYPE NVARCHAR(50),
    @P_PET_SIZE NVARCHAR(20),
    @P_PET_AGGRESSIVENESS TINYINT,
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO packages(package_name, status, description, room_id, pet_breed_type, pet_size, pet_aggressiveness, created_date, modified_date)
    OUTPUT INSERTED.package_id INTO @ID
    VALUES (@P_PACKAGE_NAME, @P_STATUS, @P_DESCRIPTION, @P_ROOM_ID, @P_PET_BREED_TYPE, @P_PET_SIZE, @P_PET_AGGRESSIVENESS, @P_CREATED_DATE, @P_MODIFIED_DATE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Add service to package
CREATE PROCEDURE ADD_SERVICE_TO_PACKAGE_PR
    @P_PACKAGE_ID INT,
    @P_SERVICE_ID INT
AS BEGIN
    INSERT INTO package_services(package_id, service_id)
    VALUES (@P_PACKAGE_ID, @P_SERVICE_ID)
END
GO

-- Remove service from package
CREATE PROCEDURE REMOVE_SERVICE_FROM_PACKAGE_PR
    @P_PACKAGE_ID INT,
    @P_SERVICE_ID INT
AS BEGIN
    DELETE FROM package_services
    WHERE package_id = @P_PACKAGE_ID AND service_id = @P_SERVICE_ID
END
GO

-- Retrieve package services by package id
CREATE PROCEDURE RETRIEVE_PACKAGE_SERVICES_BY_PACKAGE_ID_PR
    @P_PACKAGE_ID INT
AS BEGIN
    SELECT 
        sr.service_id, 
        sr.service_name, 
        sr.status, 
        sr.description, 
        sr.cost, 
        sr.created_date, 
        sr.modified_date
    FROM package_services
    INNER JOIN services sr ON sr.service_id = package_services.service_id
    WHERE package_services.package_id = @P_PACKAGE_ID
END
GO

-- Update package
CREATE PROCEDURE UPDATE_PACKAGE_PR
    @P_PACKAGE_ID INT,
    @P_PACKAGE_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(200),
    @P_ROOM_ID INT,
    @P_PET_BREED_TYPE NVARCHAR(50),
    @P_PET_SIZE NVARCHAR(20),
    @P_PET_AGGRESSIVENESS TINYINT,
    @P_MODIFIED_DATE DATETIME
AS BEGIN
    UPDATE packages
    SET package_name = @P_PACKAGE_NAME,
        status = @P_STATUS,
        description = @P_DESCRIPTION,
        room_id = @P_ROOM_ID,
        pet_breed_type = @P_PET_BREED_TYPE,
        pet_size = @P_PET_SIZE,
        pet_aggressiveness = @P_PET_AGGRESSIVENESS,
        modified_date = @P_MODIFIED_DATE
    WHERE package_id = @P_PACKAGE_ID AND status != 0
END
GO

-- Delete package
CREATE PROCEDURE DELETE_PACKAGE_PR
    @P_PACKAGE_ID INT
AS BEGIN
    UPDATE packages
    SET status = 0
    WHERE package_id = @P_PACKAGE_ID
END
GO

-- Retrieve package by id
CREATE PROCEDURE RETRIEVE_PACKAGE_BY_ID_PR
    @P_PACKAGE_ID INT
AS BEGIN
    SELECT TOP 1 
        package_id, 
        package_name, 
        status, 
        description, 
        room_id, 
        pet_breed_type, 
        pet_size, 
        pet_aggressiveness, 
        created_date, 
        modified_date
    FROM packages
    WHERE package_id = @P_PACKAGE_ID AND status != 0
END
GO

-- Retrieve all packages
CREATE PROCEDURE RETRIEVE_ALL_PACKAGES_PR AS BEGIN
    SELECT 
        package_id, 
        package_name, 
        status, 
        description, 
        room_id, 
        pet_breed_type, 
        pet_size, 
        pet_aggressiveness, 
        created_date, 
        modified_date
    FROM packages
    WHERE status != 0
END
GO

-- Retrieve available packages
CREATE PROCEDURE RETRIEVE_AVAILABLE_PACKAGES_PR AS BEGIN
    SELECT 
        package_id, 
        package_name, 
        status, 
        description, 
        room_id, 
        pet_breed_type, 
        pet_size, 
        pet_aggressiveness, 
        created_date, 
        modified_date
    FROM packages
    WHERE status = 1
END
GO


/*
	Reservation stored procedures
*/

-- Create reservation
CREATE PROCEDURE CREATE_RESERVATION_PR
    @P_START_DATE DATE,
    @P_END_DATE DATE,
    @P_USER_ID INT,
    @P_PET_ID INT,
    @P_PACKAGE_ID INT,
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO reservations(start_date, end_date, user_id, pet_id, package_id, created_date, modified_date)
    OUTPUT INSERTED.reservation_id INTO @ID
    VALUES (@P_START_DATE, @P_END_DATE, @P_USER_ID, @P_PET_ID, @P_PACKAGE_ID, @P_CREATED_DATE, @P_MODIFIED_DATE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Add unwanted service
CREATE PROCEDURE ADD_UNWANTED_SERVICE_PR
    @P_RESERVATION_ID INT,
    @P_SERVICE_ID INT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO unwanted_services(reservation_id, service_id)
    VALUES (@P_RESERVATION_ID, @P_SERVICE_ID)
END
GO

-- Update reservation
CREATE PROCEDURE UPDATE_RESERVATION_PR
    @P_RESERVATION_ID INT,
    @P_START_DATE DATE,
    @P_END_DATE DATE,
    @P_USER_ID INT,
    @P_PET_ID INT,
    @P_PACKAGE_ID INT,
    @P_MODIFIED_DATE DATETIME
AS BEGIN
    UPDATE reservations
    SET start_date = @P_START_DATE,
        end_date = @P_END_DATE,
        user_id = @P_USER_ID,
        pet_id = @P_PET_ID,
        package_id = @P_PACKAGE_ID,
        modified_date = @P_MODIFIED_DATE
    WHERE reservation_id = @P_RESERVATION_ID
END
GO

-- Remove unwanted service
CREATE PROCEDURE REMOVE_UNWANTED_SERVICE_PR
    @P_RESERVATION_ID INT,
    @P_SERVICE_ID INT
AS BEGIN
    DELETE FROM unwanted_services
    WHERE reservation_id = @P_RESERVATION_ID AND service_id = @P_SERVICE_ID
END
GO

-- Delete reservation
CREATE PROCEDURE DELETE_RESERVATION_PR
    @P_RESERVATION_ID INT
AS BEGIN
	DELETE FROM reservations
    WHERE reservation_id = @P_RESERVATION_ID
END
GO

-- Retrieve unwanted services by reservation id
CREATE PROCEDURE RETRIEVE_UNWANTED_SERVICES_BY_RESERVATION_ID_PR
    @P_RESERVATION_ID INT
AS BEGIN
    SELECT 
        sr.service_id, 
        sr.service_name, 
        sr.status, 
        sr.description, 
        sr.cost, 
        sr.created_date, 
        sr.modified_date
    FROM unwanted_services
    INNER JOIN services sr ON sr.service_id = unwanted_services.service_id
    WHERE unwanted_services.reservation_id = @P_RESERVATION_ID
END
GO

-- Retrieve reservation by id
CREATE PROCEDURE RETRIEVE_RESERVATION_BY_ID_PR
    @P_RESERVATION_ID INT
AS BEGIN
    SELECT TOP 1 
        reservation_id, 
        start_date, 
        end_date, 
        user_id, 
        pet_id, 
        package_id, 
        created_date, 
        modified_date
    FROM reservations
    WHERE reservation_id = @P_RESERVATION_ID
END
GO

-- Retrieve all reservations
CREATE PROCEDURE RETRIEVE_ALL_RESERVATIONS_PR AS BEGIN
    SELECT 
        reservation_id, 
        start_date, 
        end_date, 
        user_id, 
        pet_id, 
        package_id, 
        created_date, 
        modified_date
    FROM reservations
END
GO

-- Retrieve reservations by user id
CREATE PROCEDURE RETRIEVE_RESERVATIONS_BY_USER_ID_PR
    @P_USER_ID INT
AS BEGIN
    SELECT 
        reservation_id, 
        start_date, 
        end_date, 
        user_id, 
        pet_id, 
        package_id, 
        created_date, 
        modified_date
    FROM reservations
    WHERE user_id = @P_USER_ID
END
GO


/*
	Invoice stored procedures
*/

-- Create invoice
CREATE PROCEDURE CREATE_INVOICE_PR
    @P_INVOICE_NUMBER NVARCHAR(30),
    @P_ISSUE_DATE DATE,
    @P_DUE_DATE DATE,
    @P_USER_ID INT,
    @P_TOTAL_AMOUNT DECIMAL(10, 2),
    @P_STATUS TINYINT,
    @P_TAX_AMOUNT DECIMAL(10, 2),
    @P_DISCOUNT_CODE NVARCHAR(30),
    @P_DISCOUNT_AMOUNT DECIMAL(10, 2),
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO invoices(invoice_number, issue_date, due_date, user_id, total_amount, status, tax_amount, discount_code, discount_amount)
    OUTPUT INSERTED.invoice_id INTO @ID
    VALUES (@P_INVOICE_NUMBER, @P_ISSUE_DATE, @P_DUE_DATE, @P_USER_ID, @P_TOTAL_AMOUNT, @P_STATUS, @P_TAX_AMOUNT, @P_DISCOUNT_CODE, @P_DISCOUNT_AMOUNT)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Add invoice detail
CREATE PROCEDURE ADD_INVOICE_DETAIL_PR
    @P_INVOICE_ID INT,
    @P_SERVICE_ID INT,
    @P_PACKAGE_ID INT,
    @P_ROOM_ID INT,
    @P_RESERVATION_ID INT,
    @P_PRICE DECIMAL(10, 2),
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO invoice_details(invoice_id, service_id, package_id, room_id, reservation_id, price)
    OUTPUT INSERTED.invoice_detail_id INTO @ID
    VALUES (@P_INVOICE_ID, @P_SERVICE_ID, @P_PACKAGE_ID, @P_ROOM_ID, @P_RESERVATION_ID, @P_PRICE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Remove invoice detail
CREATE PROCEDURE REMOVE_INVOICE_DETAIL_PR
    @P_INVOICE_ID INT,
    @P_INVOICE_DETAIL_ID INT
AS BEGIN
    DELETE FROM invoice_details
    WHERE invoice_id = @P_INVOICE_ID AND invoice_detail_id = @P_INVOICE_DETAIL_ID
END
GO

-- Retrieve invoice details by invoice id
CREATE PROCEDURE RETRIEVE_INVOICE_DETAILS_BY_INVOICE_ID_PR
    @P_INVOICE_ID INT
AS BEGIN
    SELECT 
        invoice_detail_id, 
        invoice_id, 
        service_id, 
        package_id, 
        room_id, 
        reservation_id, 
        price
    FROM invoice_details
    WHERE invoice_id = @P_INVOICE_ID
END
GO

-- Update invoice
CREATE PROCEDURE UPDATE_INVOICE_PR
    @P_INVOICE_ID INT,
    @P_DUE_DATE DATE,
    @P_USER_ID INT,
    @P_TOTAL_AMOUNT DECIMAL(10, 2),
    @P_STATUS TINYINT,
    @P_TAX_AMOUNT DECIMAL(10, 2),
    @P_DISCOUNT_CODE NVARCHAR(30),
    @P_DISCOUNT_AMOUNT DECIMAL(10, 2)
AS BEGIN
    UPDATE invoices
    SET due_date = @P_DUE_DATE,
        user_id = @P_USER_ID,
        total_amount = @P_TOTAL_AMOUNT,
        status = @P_STATUS,
        tax_amount = @P_TAX_AMOUNT,
        discount_code = @P_DISCOUNT_CODE,
        discount_amount = @P_DISCOUNT_AMOUNT
    WHERE invoice_id = @P_INVOICE_ID
END
GO

-- Delete invoice
CREATE PROCEDURE DELETE_INVOICE_PR
    @P_INVOICE_ID INT
AS BEGIN
    DELETE FROM invoice_details WHERE invoice_id = @P_INVOICE_ID
    DELETE FROM invoices WHERE invoice_id = @P_INVOICE_ID
END
GO

-- Retrieve invoice by id
CREATE PROCEDURE RETRIEVE_INVOICE_BY_ID_PR
    @P_INVOICE_ID INT
AS BEGIN
    SELECT TOP 1
        invoice_id, 
        invoice_number, 
        issue_date, 
        due_date, 
        user_id, 
        total_amount, 
        status, 
        tax_amount, 
        discount_code, 
        discount_amount
    FROM invoices
    WHERE invoice_id = @P_INVOICE_ID
END
GO

-- Retrieve all invoices
CREATE PROCEDURE RETRIEVE_ALL_INVOICES_PR AS BEGIN
    SELECT
        invoice_id, 
        invoice_number, 
        issue_date, 
        due_date, 
        user_id, 
        total_amount, 
        status, 
        tax_amount, 
        discount_code, 
        discount_amount
    FROM invoices
END
GO

-- Retrieve invoices by user id
CREATE PROCEDURE RETRIEVE_INVOICES_BY_USER_ID_PR
    @P_USER_ID INT
AS BEGIN
    SELECT 
        invoice_id, 
        invoice_number, 
        issue_date, 
        due_date, 
        user_id, 
        total_amount, 
        status, 
        tax_amount, 
        discount_code, 
        discount_amount
    FROM invoices
    WHERE user_id = @P_USER_ID
END
GO


/*
	Room stored procedures
*/

-- Create room
CREATE PROCEDURE CREATE_ROOM_PR
    @P_ROOM_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_COST DECIMAL(10, 2),
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO rooms(room_name, status, description, cost, created_date, modified_date)
    OUTPUT INSERTED.room_id INTO @ID
    VALUES (@P_ROOM_NAME, @P_STATUS, @P_DESCRIPTION, @P_COST, @P_CREATED_DATE, @P_MODIFIED_DATE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Update room
CREATE PROCEDURE UPDATE_ROOM_PR
    @P_ROOM_ID INT,
    @P_ROOM_NAME NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_COST DECIMAL(10, 2),
    @P_MODIFIED_DATE DATETIME
AS BEGIN
    UPDATE rooms
    SET room_name = @P_ROOM_NAME,
        status = @P_STATUS,
        description = @P_DESCRIPTION,
        cost = @P_COST,
        modified_date = @P_MODIFIED_DATE
    WHERE room_id = @P_ROOM_ID AND status != 0
END
GO

-- Delete room
CREATE PROCEDURE DELETE_ROOM_PR
    @P_ROOM_ID INT
AS BEGIN
    UPDATE rooms
    SET status = 0
    WHERE room_id = @P_ROOM_ID
END
GO

-- Retrieve room by id
CREATE PROCEDURE RETRIEVE_ROOM_BY_ID_PR
    @P_ROOM_ID INT
AS BEGIN
    SELECT TOP 1
        room_id, 
        room_name, 
        status, 
        description, 
        cost, 
        created_date, 
        modified_date
    FROM rooms
    WHERE room_id = @P_ROOM_ID AND status != 0
END
GO

-- Retrieve all rooms
CREATE PROCEDURE RETRIEVE_ALL_ROOMS_PR AS BEGIN
    SELECT
        room_id, 
        room_name, 
        status, 
        description, 
        cost, 
        created_date, 
        modified_date
    FROM rooms
    WHERE status != 0
END
GO

-- Retrieve available rooms
CREATE PROCEDURE RETRIEVE_AVAILABLE_ROOMS_PR AS BEGIN
    SELECT 
        room_id, 
        room_name, 
        status, 
        description, 
        cost, 
        created_date, 
        modified_date
    FROM rooms
    WHERE status = 1
END
GO


/*
	Discount code stored procedures
*/

-- Create discount code
CREATE PROCEDURE CREATE_DISCOUNT_CODE_PR
    @P_CODE NVARCHAR(30),
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_STATUS TINYINT,
    @P_TOTAL_ISSUED INT,
    @P_APPLIED_COUNT INT,
    @P_DISCOUNT DECIMAL(10, 2),
    @P_CREATED_DATE DATETIME,
    @P_MODIFIED_DATE DATETIME,
    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO discount_codes(code, description, status, total_issued, applied_count, discount, created_date, modified_date)
    OUTPUT INSERTED.discount_code_id INTO @ID
    VALUES (@P_CODE, @P_DESCRIPTION, @P_STATUS, @P_TOTAL_ISSUED, @P_APPLIED_COUNT, @P_DISCOUNT, @P_CREATED_DATE, @P_MODIFIED_DATE)

    SELECT @P_ID = ID FROM @ID
END
GO

-- Update discount code
CREATE PROCEDURE UPDATE_DISCOUNT_CODE_PR
    @P_DISCOUNT_CODE_ID INT,
    @P_CODE NVARCHAR(30),
    @P_DESCRIPTION NVARCHAR(MAX),
    @P_STATUS TINYINT,
    @P_TOTAL_ISSUED INT,
    @P_APPLIED_COUNT INT,
    @P_DISCOUNT DECIMAL(10, 2),
    @P_MODIFIED_DATE DATETIME
AS BEGIN
    UPDATE discount_codes
    SET code = @P_CODE,
        description = @P_DESCRIPTION,
        status = @P_STATUS,
        total_issued = @P_TOTAL_ISSUED,
        applied_count = @P_APPLIED_COUNT,
        discount = @P_DISCOUNT,
        modified_date = @P_MODIFIED_DATE
    WHERE discount_code_id = @P_DISCOUNT_CODE_ID AND status != 0
END
GO

-- Delete discount code
CREATE PROCEDURE DELETE_DISCOUNT_CODE_PR
    @P_DISCOUNT_CODE_ID INT
AS BEGIN
    UPDATE discount_codes
    SET status = 0
    WHERE discount_code_id = @P_DISCOUNT_CODE_ID
END
GO

-- Retrieve discount code by id
CREATE PROCEDURE RETRIEVE_DISCOUNT_CODE_BY_ID_PR
    @P_DISCOUNT_CODE_ID INT
AS BEGIN
    SELECT TOP 1 
        discount_code_id, 
        code, 
        description, 
        status, 
        total_issued, 
        applied_count, 
        discount, 
        created_date, 
        modified_date
    FROM discount_codes
    WHERE discount_code_id = @P_DISCOUNT_CODE_ID AND status != 0
END
GO

-- Retrieve all discount codes
CREATE PROCEDURE RETRIEVE_ALL_DISCOUNT_CODES_PR AS BEGIN
    SELECT 
        discount_code_id, 
        code, 
        description, 
        status, 
        total_issued, 
        applied_count, 
        discount, 
        created_date, 
        modified_date
    FROM discount_codes
    WHERE status != 0
END
GO


/*
	IoT stored procedures
*/

-- Create IoT
CREATE PROCEDURE CREATE_IOT_PR
    @P_IOT_ID INT,
    @P_IOT_TYPE NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_RESERVATION_ID INT,
    @P_CREATED_DATE DATETIME,

    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO iot(iot_id, iot_type, status, reservation_id, created_date)
    OUTPUT INSERTED.iot_id INTO @ID
    VALUES (@P_IOT_ID, @P_IOT_TYPE, @P_STATUS, @P_RESERVATION_ID, @P_CREATED_DATE)

    SELECT ID FROM @ID
END
GO

-- Set Iot to reservation
CREATE PROCEDURE SET_IOT_TO_RESERVATION_PR
    @P_IOT_ID INT,
    @P_RESERVATION_ID INT
AS BEGIN
    UPDATE iot
    SET reservation_id = @P_RESERVATION_ID
    WHERE iot_id = @P_IOT_ID
END
GO    

-- Remove Iot from reservation
CREATE PROCEDURE REMOVE_IOT_FROM_RESERVATION_PR
    @P_IOT_ID INT
AS BEGIN
    UPDATE iot
    SET reservation_id = NULL
    WHERE iot_id = @P_IOT_ID
END
GO

-- Update IoT
CREATE PROCEDURE UPDATE_IOT_PR
    @P_IOT_ID INT,
    @P_IOT_TYPE NVARCHAR(50),
    @P_STATUS TINYINT,
    @P_RESERVATION_ID INT,
    @P_MODIFIED_DATE DATETIME,
	@P_CREATED_DATE DATETIME
AS BEGIN
    UPDATE iot
    SET iot_type = @P_IOT_TYPE,
        status = @P_STATUS,
        reservation_id = @P_RESERVATION_ID,
		created_date = @P_CREATED_DATE,
        modified_date = @P_MODIFIED_DATE
    WHERE iot_id = @P_IOT_ID AND status != 0
END
GO

/*
	IoT pet record stored procedures
*/

-- Create IoT pet record
CREATE PROCEDURE CREATE_IOT_PET_RECORD_PR
    @P_IOT_ID INT,
    @P_PET_ID INT,
    @P_PULSE_RATE INT,
    @P_CREATED_DATE DATETIME,
    @P_LIGHT DECIMAL(10, 2),
    @P_HUMIDITY DECIMAL(10, 2),
    @P_TEMPERATURE DECIMAL(10, 2),
    @P_GAS FLOAT,
    @P_ALTITUDE FLOAT,
    @P_PRESSURE FLOAT,
    @P_CONTADOR_DE_PASOS INT,
    @P_ID INT OUTPUT
AS 
BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO iot_pet_records(iot_id, pet_id, pulse_rate, created_date, light, humidity, temperature, gas, altitude, pressure, ContadorDePasos)
    OUTPUT INSERTED.iot_pet_record_id INTO @ID
    VALUES (@P_IOT_ID, @P_PET_ID, @P_PULSE_RATE, @P_CREATED_DATE, @P_LIGHT, @P_HUMIDITY, @P_TEMPERATURE, @P_GAS, @P_ALTITUDE, @P_PRESSURE, @P_CONTADOR_DE_PASOS)

    SELECT @P_ID = ID FROM @ID
END;
GO

CREATE PROCEDURE UPDATE_IOT_PET_RECORD_PR
    @P_IOT_RECORD_ID INT,
    @P_PULSE_RATE INT,
    @P_CREATED_DATE DATETIME,
    @P_LIGHT DECIMAL(10, 2),
    @P_HUMIDITY DECIMAL(10, 2),
    @P_TEMPERATURE DECIMAL(10, 2),
    @P_GAS FLOAT,
    @P_ALTITUDE FLOAT,
    @P_PRESSURE FLOAT,
    @P_CONTADOR_DE_PASOS INT
AS 
BEGIN
    UPDATE iot_pet_records
    SET
        pulse_rate = @P_PULSE_RATE,
        created_date = @P_CREATED_DATE,
        light = @P_LIGHT,
        humidity = @P_HUMIDITY,
        temperature = @P_TEMPERATURE,
        gas = @P_GAS,
        altitude = @P_ALTITUDE,
        pressure = @P_PRESSURE,
        ContadorDePasos = @P_CONTADOR_DE_PASOS
    WHERE
        iot_pet_record_id = @P_IOT_RECORD_ID
END;
GO


CREATE PROCEDURE IOT_PET_RECORDS_RetrieveAll
AS
BEGIN
    SELECT        
        iot_id,
        pet_id,
        pulse_rate,
        created_date,
        light,
        humidity,
        temperature,
        gas,
        altitude,
        pressure,
        ContadorDePasos
    FROM iot_pet_records;
END;
GO

CREATE PROCEDURE IOT_PET_RECORDS_RetrieveById
    @P_IOT_ID INT
AS
BEGIN
    SELECT        
        iot_id,
        pet_id,
        pulse_rate,
        created_date,
        light,
        humidity,
        temperature,
        gas,
        altitude,
        pressure,
        ContadorDePasos
    FROM iot_pet_records
    WHERE iot_id = @P_IOT_ID;
END;
GO

CREATE PROCEDURE IOT_PET_RECORDS_RetrieveByPetId
    @P_PET_ID INT
AS
BEGIN
    SELECT        
        iot_id,
        pet_id,
        pulse_rate,
        created_date,
        light,
        humidity,
        temperature,
        gas,
        altitude,
        pressure,
        ContadorDePasos
    FROM iot_pet_records
    WHERE pet_id = @P_PET_ID;
END;
GO

-- Delete IoT pet record by iot id
CREATE PROCEDURE DELETE_IOT_PET_RECORD_BY_IOT_ID_PR
    @P_IOT_ID INT
AS BEGIN
    DELETE FROM iot_pet_records
    WHERE iot_id = @P_IOT_ID
END
GO

-- Delete IoT pet record by reservation id
CREATE PROCEDURE DELETE_IOT_PET_RECORD_BY_RESERVATION_ID_PR
    @P_RESERVATION_ID INT
AS BEGIN
    DELETE iopr
    FROM iot_pet_records iopr
    INNER JOIN iot ON iot.iot_id = iopr.iot_id
    WHERE iot.reservation_id = @P_RESERVATION_ID
END
GO

/*
	IoT room record stored procedures
*/

-- Create IoT room record
CREATE PROCEDURE CREATE_IOT_ROOM_RECORD_PR
    @P_IOT_ID INT,
    @P_ROOM_ID INT,
    @P_TEMPERATURE DECIMAL(10, 2),
    @P_HUMIDITY DECIMAL(10, 2),
    @P_LIGHT DECIMAL(10, 2),
    @P_CREATED_DATE DATETIME,

    @P_ID INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

    INSERT INTO iot_room_records(iot_id, room_id, temperature, humidity, light, created_date)
    OUTPUT INSERTED.iot_room_record_id INTO @ID
    VALUES (@P_IOT_ID, @P_ROOM_ID, @P_TEMPERATURE, @P_HUMIDITY, @P_LIGHT, @P_CREATED_DATE)

    SELECT ID FROM @ID
END
GO

-- Delete IoT room record by iot id
CREATE PROCEDURE DELETE_IOT_ROOM_RECORD_BY_IOT_ID_PR
    @P_IOT_ID INT
AS BEGIN
    DELETE FROM iot_room_records
    WHERE iot_id = @P_IOT_ID
END
GO

-- Delete IoT room record by reservation id
CREATE PROCEDURE DELETE_IOT_ROOM_RECORD_BY_RESERVATION_ID_PR
    @P_RESERVATION_ID INT
AS BEGIN
    DELETE iorr
    FROM iot_room_records iorr
    INNER JOIN iot ON iot.iot_id = iorr.iot_id
    WHERE iot.reservation_id = @P_RESERVATION_ID
END
GO