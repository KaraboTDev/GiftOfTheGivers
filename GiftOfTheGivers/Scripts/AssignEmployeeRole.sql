-- AssignEmployeeRole.sql
-- Replaces 'test@example.com' with the test user's email if needed.
BEGIN TRANSACTION;
DECLARE @UserId nvarchar(450) = (SELECT Id FROM AspNetUsers WHERE UserName = 'test@example.com' OR Email = 'test@example.com');
DECLARE @RoleId nvarchar(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Employee');

IF @UserId IS NULL
BEGIN
	RAISERROR('User not found. Check the email and try again.',16,1);
	ROLLBACK TRANSACTION;
	RETURN;
END

IF @RoleId IS NULL
BEGIN
	RAISERROR('Role ''Employee'' not found. Create the role or check the role name.',16,1);
	ROLLBACK TRANSACTION;
	RETURN;
END

IF NOT EXISTS (SELECT 1 FROM AspNetUserRoles WHERE UserId = @UserId AND RoleId = @RoleId)
BEGIN
	INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);
END

COMMIT TRANSACTION;

-- Verify assignment
SELECT u.Id, u.UserName, r.Id AS RoleId, r.Name
FROM AspNetUsers u
JOIN AspNetUserRoles ur ON u.Id = ur.UserId
JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName = 'test@example.com' OR u.Email = 'test@example.com';
