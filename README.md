# 23-52116-2_LoginSystem

This is a C# Windows Forms Login and Register application connected to SQL Server. It contains `frmLogin`, `frmRegister`, and `frmDashboard`. A user can register, log in, and log out.

## How to run

1. Open `database.sql` in SQL Server Management Studio and run it.
2. It creates `db_users`, `tbl_users`, and the test account `admin / admin123`.
3. Open `23-52116-2_LoginSystem.csproj` in Visual Studio 2022.
4. The project targets .NET Framework 4.7.2.
5. `App.config` contains the connection string under `connString`.
6. If the SQL Server instance is different, change only `Data Source`.
7. Build and run the application.

## What was changed

The database access was changed from Microsoft Access/OleDb to SQL Server/SqlClient. `System.Data.SqlClient` is used for `SqlConnection` and `SqlCommand`.

The connection string is stored in `App.config` and read with `ConfigurationManager`, so it is not repeated in the forms.

The login and registration queries use `@username` and `@password` parameters. Parameters keep entered values separate from SQL command text and help prevent SQL injection.

The dashboard Logout button closes the dashboard and returns to the login screen.

## Test

Use `admin / admin123` for the initial login. Then register a new user and test that new account. Finally, run `SELECT * FROM tbl_users;` in SQL Server to confirm the registered user is stored.

## Submission note

The instructor's resubmission instructions ask for the README to be in the student's own words. Review and edit this README before final submission so it accurately reflects the student's own testing and explanation.
