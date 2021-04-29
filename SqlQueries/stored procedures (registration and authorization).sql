

-- result = 0 - success
-- result = 1 - user with such login already exist
DROP PROCEDURE add_user
GO
CREATE PROCEDURE add_user
	@login NVARCHAR(max),
	@first_name NVARCHAR(max),
	@second_name NVARCHAR(max) = NULL,
	@middle_name NVARCHAR(max) = NULL,
	@hashsum NVARCHAR(max),
	@employee_id INT OUTPUT,
	@result INT OUTPUT
AS
BEGIN
	IF (SELECT count(*) FROM Employees WHERE Employees.login = @login) = 0
	BEGIN
		INSERT INTO Employees (login, first_name, second_name, middle_name, hashsum) VALUES
		(@login, @first_name, @second_name, @middle_name, @hashsum);
		SELECT @result = 0
		SELECT @employee_id = (SELECT id FROM Employees WHERE login = @login)
		RETURN
	END
	ELSE
	BEGIN
		SELECT @result = 1
		RETURN
	END
END


/*
-- result = 0 - success
-- result = 1 - invalid login
-- result = 2 - invalid password
GO
CREATE PROCEDURE login_user
	@login NVARCHAR(max),
	@hashsum NVARCHAR(max),
	@result INT OUTPUT
AS
BEGIN
	IF (SELECT count(*) FROM Employees WHERE Employees.login = @login) != 1
	BEGIN
		SELECT @result = 1
		RETURN
	END
	ELSE IF (SELECT hashsum from Employees WHERE login = @login) = @hashsum
	BEGIN
		SELECT @result = 0
		RETURN
	END
	ELSE
	BEGIN
		SELECT @result = 2
		RETURN
	END
END
*/


