/* 
	This script is used to create the stored procedures for the project.
*/


/*
    User stored procedures 
*/

-- Create user
-- LISTO
CREATE PROCEDURE CREATE_USER_PR
	@IsOtpVerified BIT,
	@PasswordHash NVARCHAR(MAX),
    @Role NVARCHAR(20),
    @Status TINYINT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @IdentificationType NVARCHAR(30),
    @IdentifierValue NVARCHAR(60),
    @Email NVARCHAR(50),
    @ProfilePhotoUrl NVARCHAR(MAX),
    @ThemePreference NVARCHAR(20),
    @CreatedDate DATETIME,
    @ModifiedDate DATETIME,
    @AddressLatitude FLOAT,
    @AddressLongitude FLOAT,

    @Id INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

	INSERT INTO users(is_otp_verified, password_hash, role, status, first_name, last_name, identification_type, identifier_value, email, profile_photo_url, theme_preference, created_date, modified_date, address_latitude, address_longitude)
    OUTPUT INSERTED.user_id INTO @ID
	VALUES (@IsOtpVerified, @PasswordHash, @Role, @Status, @FirstName, @LastName, @IdentificationType, @IdentifierValue, @Email, @ProfilePhotoUrl, @ThemePreference, @CreatedDate, @ModifiedDate, @AddressLatitude, @AddressLongitude)

    SELECT @Id = ID FROM @ID
END
GO

-- Add phone
-- LISTO
CREATE PROCEDURE ADD_PHONE_NUMBERS_TO_USER_PR
	@UserId INT,
	@PhoneNumber NVARCHAR(20),

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO phones(user_id, phone_number)
	OUTPUT INSERTED.phone_id INTO @ID
	VALUES (@UserId, @PhoneNumber)

	SELECT @Id = ID FROM @ID
END
GO

-- Remove phone
-- LISTO
CREATE PROCEDURE REMOVE_PHONE_NUMBERS_TO_USER_PR
	@UserId INT,
	@PhoneId INT
AS BEGIN
	DELETE FROM phones
	WHERE user_id = @UserId AND phone_id = @PhoneId
END
GO

-- Retrieve phones by user id
-- LISTO
CREATE PROCEDURE RETRIEVE_PHONE_NUMBERS_BY_USER_ID_PR
	@UserId INT
AS BEGIN
	SELECT phone_number
	FROM phones
	WHERE user_id = @UserId
END
GO

-- Update user
-- LISTO
CREATE PROCEDURE UPDATE_USER_PR
	@UserId INT,	
	@Role NVARCHAR(20),
    @Status TINYINT,
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@IdentificationType NVARCHAR(30),
	@IdentifierValue NVARCHAR(60),
	@Email NVARCHAR(50),
	@ProfilePhotoUrl NVARCHAR(MAX),
	@ThemePreference NVARCHAR(20),	
	@ModifiedDate DATETIME,
	@AddressLatitude FLOAT,
	@AddressLongitude FLOAT
AS BEGIN
	UPDATE users
	SET role = @Role,
        status = @Status,
		first_name = @FirstName,
		last_name = @LastName,
		identification_type = @IdentificationType,
		identifier_value = @IdentifierValue,
		email = @Email,
		profile_photo_url = @ProfilePhotoUrl,
		theme_preference = @ThemePreference,
		modified_date = @ModifiedDate,
		address_latitude = @AddressLatitude,
		address_longitude = @AddressLongitude
	WHERE user_id = @UserId AND status != 0
END
GO

-- Delete user
-- Listo
CREATE PROCEDURE DELETE_USER_PR
	@UserId INT,
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE users
    SET status = 0,
	modified_date = @ModifiedDate
    WHERE user_id = @UserId
END
GO

-- Retrieve user by id
-- LISTO
CREATE PROCEDURE RETRIEVE_USER_BY_ID_PR
	@UserId INT
AS BEGIN
	SELECT TOP 1 user_id, is_otp_verified, password_hash, role, status, first_name, last_name, identification_type, identifier_value, email, profile_photo_url, theme_preference, created_date, modified_date, address_latitude, address_longitude
    FROM users
	WHERE user_id = @UserId AND status != 0
END
GO

-- Retrieve user by email
-- LISTO
CREATE PROCEDURE RETRIEVE_USER_BY_EMAIL_PR
	@Email NVARCHAR(50)
AS BEGIN
	SELECT TOP 1 user_id, is_otp_verified, password_hash, role, status, first_name, last_name, identification_type, identifier_value, email, profile_photo_url, theme_preference, created_date, modified_date, address_latitude, address_longitude
    FROM users
	WHERE email = @Email AND status != 0
END
GO

-- Retrieve all users
-- LISTO
CREATE PROCEDURE RETRIEVE_ALL_USERS_PR AS BEGIN
	SELECT user_id, is_otp_verified, password_hash, role, status, first_name, last_name, identification_type, identifier_value, email, profile_photo_url, theme_preference, created_date, modified_date, address_latitude, address_longitude
    FROM users
    WHERE status != 0
END
GO

/* 
    Pet stored procedures
*/

-- Create pet
CREATE PROCEDURE CREATE_PET_PR
	@PetName NVARCHAR(50),
    @Status TINYINT,
	@Description NVARCHAR(MAX),
	@Age TINYINT,
	@Breed NVARCHAR(50),
	@Aggressiveness TINYINT,
	@CreatedDate DATETIME,
	@ModifiedDate DATETIME,
	@UserId INT,

	@Id INT OUTPUT
AS BEGIN
    DECLARE @ID TABLE (ID INT)

	INSERT INTO pets(pet_name, status, description, age, breed, aggressiveness, created_date, modified_date, user_id)
	OUTPUT INSERTED.pet_id INTO @ID
	VALUES (@PetName, @Status, @Description, @Age, @Breed, @Aggressiveness, @CreatedDate, @ModifiedDate, @UserId)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Add pet pic
-- LISTO
CREATE PROCEDURE ADD_PHOTOS_TO_PET_PR
	@PetId INT,
	@PhotoUrl NVARCHAR(MAX),

    @Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO pet_pics(pet_id, photo_url)
    OUTPUT INSERTED.pet_photo_id INTO @ID
	VALUES (@PetId, @PhotoUrl)

    RETURN (SELECT ID FROM @ID)
END
GO

-- Remove pet pic
CREATE PROCEDURE REMOVE_PET_PHOTO_PR
	@PetId INT,
	@PicId INT
AS BEGIN
	DELETE FROM pet_pics
	WHERE pet_id = @PetId AND pic_id = @PicId
END
GO

-- Retrieve pet photos by pet id
CREATE PROCEDURE RETRIEVE_PHOTOS_BY_PET_ID_PR
	@PetId INT
AS BEGIN
	SELECT pic_id, pet_id, pic_url
	FROM pet_pics
	WHERE pet_id = @PetId
END
GO

-- Update pet
CREATE PROCEDURE UPDATE_PET_PR
	@PetId INT,
	@PetName NVARCHAR(50),    
	@Description NVARCHAR(MAX),
    @Status TINYINT,
	@Age TINYINT,
	@Breed NVARCHAR(50),
	@Aggressiveness TINYINT,
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE pets
	SET pet_name = @PetName,
		description = @Description,
		age = @Age,
		breed = @Breed,
		aggressiveness = @Aggressiveness,
		modified_date = @ModifiedDate
	WHERE pet_id = @PetId AND status != 0
END
GO

-- Delete pet
-- LISTO
CREATE PROCEDURE DELETE_PET_PR
	@PetId INT
AS BEGIN
	UPDATE pets
	SET status = 0
	WHERE pet_id = @PetId
END
GO

-- Retrieve pet by id
-- LISTO
CREATE PROCEDURE RETRIEVE_PET_BY_ID_PR
	@PetId INT
AS BEGIN
	SELECT TOP 1 pet_id, pet_name, status, description, age, breed, aggressiveness, created_date, modified_date, user_id
    FROM pets
	WHERE pet_id = @PetId AND status != 0
END
GO

-- Retrieve all pets
-- LISTO
CREATE PROCEDURE RETRIEVE_ALL_PETS_PR AS BEGIN
	SELECT pet_id, pet_name, status, description, age, breed, aggressiveness, created_date, modified_date, user_id
    FROM pets 
    WHERE status != 0
END
GO

-- Retrieve pets by user id
CREATE PROCEDURE RETRIEVE_PETS_BY_USER_ID_PR
	@UserId INT
AS BEGIN
	SELECT TOP 1 pet_id, pet_name, status, description, age, breed, aggressiveness, created_date, modified_date, user_id
    FROM pets
	WHERE user_id = @UserId AND status != 0
END
GO

/*
	Service stored procedures
*/

-- Create service
CREATE PROCEDURE CREATE_SERVICE_PR
	@ServiceName NVARCHAR(50),
	@Status TINYINT,
	@Description NVARCHAR(MAX),
	@Cost DECIMAL(10, 2),
	@CreatedDate DATETIME,
	@ModifiedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

    INSERT INTO services(service_name, status, description, cost, created_date, modified_date)
    OUTPUT INSERTED.service_id INTO @ID
    VALUES (@ServiceName, @Status, @Description, @Cost, @CreatedDate, @ModifiedDate)

    RETURN (SELECT ID FROM @ID)
END
GO

-- Update service
CREATE PROCEDURE UPDATE_SERVICE_PR
	@ServiceId INT,
	@ServiceName NVARCHAR(50),
	@Status TINYINT,
	@Description NVARCHAR(MAX),
	@Cost DECIMAL(10, 2),
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE services
	SET service_name = @ServiceName,
		status = @Status,
		description = @Description,
		cost = @Cost,
		modified_date = @ModifiedDate
	WHERE service_id = @ServiceId  AND status != 0
END
GO

-- Delete service
CREATE PROCEDURE DELETE_SERVICE_PR
	@ServiceId INT
AS BEGIN
    UPDATE services
	SET status = 0
	WHERE service_id = @ServiceId
END
GO

-- Retrieve service by id
CREATE PROCEDURE RETRIEVE_SERVICE_BY_ID_PR
	@ServiceId INT
AS BEGIN
	SELECT service_id, service_name, status, description, cost, created_date, modified_date
	FROM services
	WHERE service_id = @ServiceId AND status != 0
END
GO

-- Retrieve all services
CREATE PROCEDURE RETRIEVE_ALL_SERVICES_PR AS BEGIN
	SELECT service_id, service_name, status, description, cost, created_date, modified_date
	FROM services
	WHERE status != 0
END
GO

-- Retrieve available services
CREATE PROCEDURE RETRIEVE_AVAILABLE_SERVICES_PR AS BEGIN
	SELECT service_id, service_name, status, description, cost, created_date, modified_date
	FROM services
	WHERE status = 1
END
GO

/*
	Package stored procedures
*/

-- Create package
CREATE PROCEDURE CREATE_PACKAGE_PR
	@PackageName NVARCHAR(50),
	@Status TINYINT,
	@Description NVARCHAR(200),
	@RoomId INT,
	@PetBreedType NVARCHAR(50),
	@PetSize NVARCHAR(20),
	@PetAggressiveness TINYINT,
	@CreatedDate DATETIME,
	@ModifiedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO packages(package_name, status, description, room_id, pet_breed_type, pet_size, pet_aggressiveness, created_date, modified_date)
	OUTPUT INSERTED.package_id INTO @ID
	VALUES (@PackageName, @Status, @Description, @RoomId, @PetBreedType, @PetSize, @PetAggressiveness, @CreatedDate, @ModifiedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Add service to package
CREATE PROCEDURE ADD_SERVICE_TO_PACKAGE_PR
	@PackageId INT,
	@ServiceId INT,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO package_services(package_id, service_id)
	OUTPUT INSERTED.package_id INTO @ID
	VALUES (@PackageId, @ServiceId)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Remove service from package
CREATE PROCEDURE REMOVE_SERVICE_FROM_PACKAGE_PR
	@PackageId INT,
	@ServiceId INT
AS BEGIN
	DELETE FROM package_services
	WHERE package_id = @PackageId AND service_id = @ServiceId
END
GO

-- Retrieve package services by package id
CREATE PROCEDURE RETRIEVE_PACKAGE_SERVICES_BY_PACKAGE_ID_PR
	@PackageId INT
AS BEGIN
	SELECT sr.service_id, sr.service_name, sr.status, sr.description, sr.cost, sr.created_date, sr.modified_date
	FROM package_services
	INNER JOIN services sr ON sr.service_id = package_services.service_id
	WHERE package_id = @PackageId
END
GO

-- Update package
CREATE PROCEDURE UPDATE_PACKAGE_PR
	@PackageId INT,
	@PackageName NVARCHAR(50),
	@Status TINYINT,
	@Description NVARCHAR(200),
	@RoomId INT,
	@PetBreedType NVARCHAR(50),
	@PetSize NVARCHAR(20),
	@PetAggressiveness TINYINT,
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE packages
	SET package_name = @PackageName,
		status = @Status,
		description = @Description,
		room_id = @RoomId,
		pet_breed_type = @PetBreedType,
		pet_size = @PetSize,
		pet_aggressiveness = @PetAggressiveness,
		modified_date = @ModifiedDate
	WHERE package_id = @PackageId AND status != 0
END
GO

-- Delete package
CREATE PROCEDURE DELETE_PACKAGE_PR
	@PackageId INT
AS BEGIN
	UPDATE packages
	SET status = 0
	WHERE package_id = @PackageId
END
GO

-- Retrieve package by id
CREATE PROCEDURE RETRIEVE_PACKAGE_BY_ID_PR
	@PackageId INT
AS BEGIN
	SELECT TOP 1 package_id, package_name, status, description, room_id, pet_breed_type, pet_size, pet_aggressiveness, created_date, modified_date
	FROM packages
	WHERE package_id = @PackageId AND status != 0
END
GO

-- Retrieve all packages
CREATE PROCEDURE RETRIEVE_ALL_PACKAGES_PR AS BEGIN
	SELECT package_id, package_name, status, description, room_id, pet_breed_type, pet_size, pet_aggressiveness, created_date, modified_date
	FROM packages
	WHERE status != 0
END
GO

-- Retrieve available packages
CREATE PROCEDURE RETRIEVE_AVAILABLE_PACKAGES_PR AS BEGIN
	SELECT package_id, package_name, status, description, room_id, pet_breed_type, pet_size, pet_aggressiveness, created_date, modified_date
	FROM packages
	WHERE status = 1
END
GO

/*
	Reservation stored procedures
*/

-- Create reservation
CREATE PROCEDURE CREATE_RESERVATION_PR
	@StartDate DATE,
	@EndDate DATE,
	@UserId INT,
	@PetId INT,
	@PackageId INT,
	@CreatedDate DATETIME,
	@ModifiedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO reservations(start_date, end_date, user_id, pet_id, package_id, created_date, modified_date)
	OUTPUT INSERTED.reservation_id INTO @ID
	VALUES (@StartDate, @EndDate, @UserId, @PetId, @PackageId, @CreatedDate, @ModifiedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Add unwanted service
CREATE PROCEDURE ADD_UNWANTED_SERVICE_PR
	@ReservationId INT,
	@ServiceId INT,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO unwanted_services(reservation_id, service_id)
	OUTPUT INSERTED.reservation_id INTO @ID
	VALUES (@ReservationId, @ServiceId)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Update reservation
CREATE PROCEDURE UPDATE_RESERVATION_PR
	@ReservationId INT,
	@StartDate DATE,
	@EndDate DATE,
	@UserId INT,
	@PetId INT,
	@PackageId INT,
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE reservations
	SET start_date = @StartDate,
		end_date = @EndDate,
		user_id = @UserId,
		pet_id = @PetId,
		package_id = @PackageId,
		modified_date = @ModifiedDate
	WHERE reservation_id = @ReservationId
END
GO

-- Remove unwanted service
CREATE PROCEDURE REMOVE_UNWANTED_SERVICE_PR
	@ReservationId INT,
	@ServiceId INT
AS BEGIN
	DELETE FROM unwanted_services
	WHERE reservation_id = @ReservationId AND service_id = @ServiceId
END
GO

-- Delete reservation
CREATE PROCEDURE DELETE_RESERVATION_PR
	@ReservationId INT
AS BEGIN
	UPDATE reservations
	SET status = 0
	WHERE reservation_id = @ReservationId
END
GO

-- Retrieve unwanted services by reservation id
CREATE PROCEDURE RETRIEVE_UNWANTED_SERVICES_BY_RESERVATION_ID_PR
	@ReservationId INT
AS BEGIN
	SELECT sr.service_id, sr.service_name, sr.status, sr.description, sr.cost, sr.created_date, sr.modified_date
	FROM unwanted_services
	INNER JOIN services sr ON sr.service_id = unwanted_services.service_id
	WHERE reservation_id = @ReservationId
END
GO

-- Retrieve reservation by id
CREATE PROCEDURE RETRIEVE_RESERVATION_BY_ID_PR
	@ReservationId INT
AS BEGIN
	SELECT TOP 1 reservation_id, start_date, end_date, user_id, pet_id, package_id, created_date, modified_date
	FROM reservations
	WHERE reservation_id = @ReservationId
END
GO

-- Retrieve all reservations
CREATE PROCEDURE RETRIEVE_ALL_RESERVATIONS_PR AS BEGIN
	SELECT reservation_id, start_date, end_date, user_id, pet_id, package_id, created_date, modified_date
	FROM reservations
END
GO

-- Retrieve reservations by user id
CREATE PROCEDURE RETRIEVE_RESERVATIONS_BY_USER_ID_PR
	@UserId INT
AS BEGIN
	SELECT reservation_id, start_date, end_date, user_id, pet_id, package_id, created_date, modified_date
	FROM reservations
	WHERE user_id = @UserId
END
GO

/*
	Invoice stored procedures
*/

-- Create invoice
CREATE PROCEDURE CREATE_INVOICE_PR
	@InvoiceNumber NVARCHAR(30),
	@IssueDate DATE,
	@DueDate DATE,
	@UserId INT,
	@TotalAmount DECIMAL(10, 2),
	@Status TINYINT,
	@TaxAmount DECIMAL(10, 2),
	@DiscountCode NVARCHAR(30),
	@DiscountAmount DECIMAL(10, 2),

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO invoices(invoice_number, issue_date, due_date, user_id, total_amount, status, tax_amount, discount_code, discount_amount)
	OUTPUT INSERTED.invoice_id INTO @ID
	VALUES (@InvoiceNumber, @IssueDate, @DueDate, @UserId, @TotalAmount, @Status, @TaxAmount, @DiscountCode, @DiscountAmount)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Add invoice detail
CREATE PROCEDURE ADD_INVOICE_DETAIL_PR
	@InvoiceId INT,
	@ServiceId INT,
	@PackageId INT,
	@RoomId INT,
	@ReservationId INT,
	@Price DECIMAL(10, 2),

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO invoice_details(invoice_id, service_id, package_id, room_id, reservation_id, price)
	OUTPUT INSERTED.invoice_detail_id INTO @ID
	VALUES (@InvoiceId, @ServiceId, @PackageId, @RoomId, @ReservationId, @Price)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Remove invoice detail
CREATE PROCEDURE REMOVE_INVOICE_DETAIL_PR
	@InvoiceId INT,
	@InvoiceDetailId INT
AS BEGIN
	DELETE FROM invoice_details
	WHERE invoice_id = @InvoiceId AND invoice_detail_id = @InvoiceDetailId
END
GO

-- Retrieve invoice details by invoice id
CREATE PROCEDURE RETRIEVE_INVOICE_DETAILS_BY_INVOICE_ID_PR
	@InvoiceId INT
AS BEGIN
	SELECT invoice_detail_id, invoice_id, service_id, package_id, room_id, reservation_id, price
	FROM invoice_details
	WHERE invoice_id = @InvoiceId
END
GO

-- Update invoice
CREATE PROCEDURE UPDATE_INVOICE_PR
	@InvoiceId INT,
	@DueDate DATE,
	@UserId INT,
	@TotalAmount DECIMAL(10, 2),
	@Status TINYINT,
	@TaxAmount DECIMAL(10, 2),
	@DiscountCode NVARCHAR(30),
	@DiscountAmount DECIMAL(10, 2)
AS BEGIN
	UPDATE invoices
	SET due_date = @DueDate,
		user_id = @UserId,
		total_amount = @TotalAmount,
		status = @Status,
		tax_amount = @TaxAmount,
		discount_code = @DiscountCode,
		discount_amount = @DiscountAmount
	WHERE invoice_id = @InvoiceId
END
GO

-- Delete invoice
CREATE PROCEDURE DELETE_INVOICE_PR
	@InvoiceId INT
AS BEGIN
	DELETE FROM invoice_details	WHERE invoice_id = @InvoiceId
	DELETE FROM invoices WHERE invoice_id = @InvoiceId
END
GO

-- Retrieve invoice by id
CREATE PROCEDURE RETRIEVE_INVOICE_BY_ID_PR
	@InvoiceId INT
AS BEGIN
	SELECT TOP 1 invoice_id, invoice_number, issue_date, due_date, user_id, total_amount, status, tax_amount, discount_code, discount_amount
	FROM invoices
	WHERE invoice_id = @InvoiceId
END
GO

-- Retrieve all invoices
CREATE PROCEDURE RETRIEVE_ALL_INVOICES_PR AS BEGIN
	SELECT invoice_id, invoice_number, issue_date, due_date, user_id, total_amount, status, tax_amount, discount_code, discount_amount
	FROM invoices
END
GO

-- Retrieve invoices by user id
CREATE PROCEDURE RETRIEVE_INVOICES_BY_USER_ID_PR
	@UserId INT
AS BEGIN
	SELECT invoice_id, invoice_number, issue_date, due_date, user_id, total_amount, status, tax_amount, discount_code, discount_amount
	FROM invoices
	WHERE user_id = @UserId
END
GO

/*
	Room stored procedures
*/

-- Create room
CREATE PROCEDURE CREATE_ROOM_PR
	@RoomName NVARCHAR(50),
	@Status TINYINT,
	@Description NVARCHAR(MAX),
	@Cost DECIMAL(10, 2),
	@CreatedDate DATETIME,
	@ModifiedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO rooms(room_name, status, description, cost, created_date, modified_date)
	OUTPUT INSERTED.room_id INTO @ID
	VALUES (@RoomName, @Status, @Description, @Cost, @CreatedDate, @ModifiedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Update room
CREATE PROCEDURE UPDATE_ROOM_PR
	@RoomId INT,
	@RoomName NVARCHAR(50),
	@Status TINYINT,
	@Description NVARCHAR(MAX),
	@Cost DECIMAL(10, 2),
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE rooms
	SET room_name = @RoomName,
		status = @Status,
		description = @Description,
		cost = @Cost,
		modified_date = @ModifiedDate
	WHERE room_id = @RoomId AND status != 0
END
GO

-- Delete room
CREATE PROCEDURE DELETE_ROOM_PR
	@RoomId INT
AS BEGIN
	UPDATE rooms
	SET status = 0
	WHERE room_id = @RoomId
END
GO

-- Retrieve room by id
CREATE PROCEDURE RETRIEVE_ROOM_BY_ID_PR
	@RoomId INT
AS BEGIN
	SELECT TOP 1 room_id, room_name, status, description, cost, created_date, modified_date
	FROM rooms
	WHERE room_id = @RoomId AND status != 0
END
GO

-- Retrieve all rooms
CREATE PROCEDURE RETRIEVE_ALL_ROOMS_PR AS BEGIN
	SELECT room_id, room_name, status, description, cost, created_date, modified_date
	FROM rooms
	WHERE status != 0
END
GO

-- Retrieve available rooms
CREATE PROCEDURE RETRIEVE_AVAILABLE_ROOMS_PR AS BEGIN
	SELECT room_id, room_name, status, description, cost, created_date, modified_date
	FROM rooms
	WHERE status = 1
END
GO

/*
	Discount code stored procedures
*/

-- Create discount code
CREATE PROCEDURE CREATE_DISCOUNT_CODE_PR
	@Code NVARCHAR(30),
	@Description NVARCHAR(MAX),
	@Status TINYINT,
	@TotalIssued INT,
	@AppliedCount INT,
	@Discount DECIMAL(10, 2),
	@CreatedDate DATETIME,
	@ModifiedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO discount_codes(code, description, status, total_issued, applied_count, discount, created_date, modified_date)
	OUTPUT INSERTED.discount_code_id INTO @ID
	VALUES (@Code, @Description, @Status, @TotalIssued, @AppliedCount, @Discount, @CreatedDate, @ModifiedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Update discount code
CREATE PROCEDURE UPDATE_DISCOUNT_CODE_PR
	@DiscountCodeId INT,
	@Code NVARCHAR(30),
	@Description NVARCHAR(MAX),
	@Status TINYINT,
	@TotalIssued INT,
	@AppliedCount INT,
	@Discount DECIMAL(10, 2),
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE discount_codes
	SET code = @Code,
		description = @Description,
		status = @Status,
		total_issued = @TotalIssued,
		applied_count = @AppliedCount,
		discount = @Discount,
		modified_date = @ModifiedDate
	WHERE discount_code_id = @DiscountCodeId AND status != 0
END
GO

-- Delete discount code
CREATE PROCEDURE DELETE_DISCOUNT_CODE_PR
	@DiscountCodeId INT
AS BEGIN
	UPDATE discount_codes
	SET status = 0
	WHERE discount_code_id = @DiscountCodeId
END
GO

-- Retrieve discount code by id
CREATE PROCEDURE RETRIEVE_DISCOUNT_CODE_BY_ID_PR
	@DiscountCodeId INT
AS BEGIN
	SELECT TOP 1 discount_code_id, code, description, status, total_issued, applied_count, discount, created_date, modified_date
	FROM discount_codes
	WHERE discount_code_id = @DiscountCodeId AND status != 0
END
GO

-- Retrieve all discount codes
CREATE PROCEDURE RETRIEVE_ALL_DISCOUNT_CODES_PR AS BEGIN
	SELECT discount_code_id, code, description, status, total_issued, applied_count, discount, created_date, modified_date
	FROM discount_codes
	WHERE status != 0
END
GO

/*
	IoT stored procedures
*/

-- Create IoT
CREATE PROCEDURE CREATE_IOT_PR
	@IotId INT,
	@IotType NVARCHAR(50),
	@Status TINYINT,
	@ReservationId INT,
	@CreatedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO iot(iot_id, iot_type, status, reservation_id, created_date)
	OUTPUT INSERTED.iot_id INTO @ID
	VALUES (@IotId, @IotType, @Status, @ReservationId, @CreatedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Set Iot to reservation
CREATE PROCEDURE SET_IOT_TO_RESERVATION_PR
	@IotId INT,
	@ReservationId INT
AS BEGIN
	UPDATE iot
	SET reservation_id = @ReservationId
	WHERE iot_id = @IotId
END
Go	

-- Remove Iot from reservation
CREATE PROCEDURE REMOVE_IOT_FROM_RESERVATION_PR
	@IotId INT
AS BEGIN
	UPDATE iot
	SET reservation_id = NULL
	WHERE iot_id = @IotId
END
GO

-- Update IoT
CREATE PROCEDURE UPDATE_IOT_PR
	@IotId INT,
	@IotType NVARCHAR(50),
	@Status TINYINT,
	@ReservationId INT,
	@ModifiedDate DATETIME
AS BEGIN
	UPDATE iot
	SET iot_type = @IotType,
		status = @Status,
		reservation_id = @ReservationId,
		modified_date = @ModifiedDate
	WHERE iot_id = @IotId AND status != 0
END
GO


/* 
	IoT pet record stored procedures
*/

-- Create IoT room record
CREATE PROCEDURE CREATE_IOT_ROOM_RECORD_PR
	@IotId INT,
	@RoomId INT,
	@Tempurate DECIMAL(10, 2),
	@Humidity DECIMAL(10, 2),
	@Light DECIMAL(10, 2),
	@CreatedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO iot_room_records(iot_id, room_id, tempurate, humidity, light, created_date)
	OUTPUT INSERTED.iot_room_record_id INTO @ID
	VALUES (@IotId, @RoomId, @Tempurate, @Humidity, @Light, @CreatedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Delete IoT by id
CREATE PROCEDURE DELETE_IOT_BY_ID_PR
	@IotId INT
AS BEGIN
	UPDATE iot
	SET status = 0
	WHERE iot_id = @IotId
END
GO

-- Retrieve IoT by id
CREATE PROCEDURE RETRIEVE_IOT_BY_ID_PR
	@IotId INT
AS BEGIN
	SELECT TOP 1 iot_id, iot_type, status, reservation_id, created_date, modified_date
	FROM iot
	WHERE iot_id = @IotId AND status != 0
END
GO

-- Retrieve all IoTs
CREATE PROCEDURE RETRIEVE_ALL_IOTS_PR AS BEGIN
	SELECT iot_id, iot_type, status, reservation_id, created_date, modified_date
	FROM iot
	WHERE status != 0
END
GO

-- Retrieve available IoTs
CREATE PROCEDURE RETRIEVE_AVAILABLE_IOTS_PR AS BEGIN
	SELECT iot_id, iot_type, status, reservation_id, created_date, modified_date
	FROM iot
	WHERE status = 1 AND reservation_id IS NULL
END
GO

/*
	IoT pet record stored procedures
*/

-- Create IoT pet record
CREATE PROCEDURE CREATE_IOT_PET_RECORD_PR
	@IotId INT,
	@PetId INT,
	@PulseRate INT,
	@CreatedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO iot_pet_records(iot_id, pet_id, pulse_rate, created_date)
	OUTPUT INSERTED.iot_pet_record_id INTO @ID
	VALUES (@IotId, @PetId, @PulseRate, @CreatedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Delete IoT pet record by iot id
CREATE PROCEDURE DELETE_IOT_PET_RECORD_BY_IOT_ID_PR
	@IotId INT
AS BEGIN
	DELETE FROM iot_pet_records
	WHERE iot_id = @IotId
END
GO

-- Delete IoT pet record by reservation id
CREATE PROCEDURE DELETE_IOT_PET_RECORD_BY_RESERVATION_ID_PR
	@ReservationId INT
AS BEGIN
	DELETE iopr
    FROM iot_pet_records iopr
    INNER JOIN iot ON iot.iot_id = iopr.iot_id
    WHERE iot.reservation_id = @ReservationId
END
GO

-- Create IoT room record
CREATE PROCEDURE CREATE_IOT_ROOM_RECORD_PR
	@IotId INT,
	@RoomId INT,
	@Tempurate DECIMAL(10, 2),
	@Humidity DECIMAL(10, 2),
	@Light DECIMAL(10, 2),
	@CreatedDate DATETIME,

	@Id INT OUTPUT
AS BEGIN
	DECLARE @ID TABLE (ID INT)

	INSERT INTO iot_room_records(iot_id, room_id, tempurate, humidity, light, created_date)
	OUTPUT INSERTED.iot_room_record_id INTO @ID
	VALUES (@IotId, @RoomId, @Tempurate, @Humidity, @Light, @CreatedDate)

	RETURN (SELECT ID FROM @ID)
END
GO

-- Delete IoT room record by iot id
CREATE PROCEDURE DELETE_IOT_ROOM_RECORD_BY_IOT_ID_PR
	@IotId INT
AS BEGIN
	DELETE FROM iot_room_records
	WHERE iot_id = @IotId
END
GO

-- Delete IoT room record by reservation id
CREATE PROCEDURE DELETE_IOT_ROOM_RECORD_BY_RESERVATION_ID_PR
	@ReservationId INT
AS BEGIN
	DELETE iorr
	FROM iot_room_records iorr
	INNER JOIN iot ON iot.iot_id = iorr.iot_id
	WHERE iot.reservation_id = @ReservationId
END
GO