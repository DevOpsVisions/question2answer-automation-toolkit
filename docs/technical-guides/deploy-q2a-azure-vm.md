# Deploying or Restoring Question2Answer on an Azure VM

This guide provides a comprehensive walkthrough for setting up a testing environment on Azure by deploying or restoring Question2Answer on an Ubuntu VM. The instructions cover preparing a LAMP stack environment, configuring essential components, and deploying the Question2Answer application. Follow these steps to ensure a smooth deployment and configuration process for testing purposes.

## 1. Preparing a LAMP Stack Environment

To prepare your environment, refer to the following guide:

[Preparing a LAMP Stack Environment for Hosting](https://github.com/DevOpsVisions/common-workspace-hub/blob/main/docs/technical-guides/prepare-lamp-stack-env.md)

## 2. Download and Set Up Question2Answer

Download the Question2Answer zip file, whether it's the latest version or a backup from your production environment.

- **Unzip the Question2Answer Archive:** Extract the downloaded Question2Answer zip file.

```bash
unzip /home/azureuser/Downloads/question2answer-latest.zip
```

- **Clean the Web Directory:** Remove any existing files in the Apache web directory to ensure a clean installation.

```bash
sudo rm -r /var/www/html/*
```

- **Copy Question2Answer to the Web Directory:** Copy the extracted Question2Answer files to the Apache web directory.

```bash
sudo cp -r /home/azureuser/Downloads/question2answer-1.8.8/public_html/. /var/www/html
```

- **Set Ownership and Permissions:** Adjust the ownership and permissions to ensure the files are accessible by the web server.

```bash
sudo chown -R www-data:www-data /var/www/html/
```
```bash
sudo chmod -R 755 /var/www/html/
```

## 3. Configure MySQL for Question2Answer

- **Log in to MySQL:** Log in to MySQL with the root user.

```bash
sudo mysql -p
```
*This command opens the MySQL command line interface. The `-p` option prompts you to enter your MySQL root password.*

- **List all MySQL users:**

```sql
SELECT User, Host FROM mysql.user;
```
*This SQL query displays all existing users and their host permissions in the MySQL server.*

- **Create a new MySQL user for Question2Answer:**

```sql
CREATE USER 'q2aUser'@'localhost' IDENTIFIED BY 'password';
```
*Creates a new MySQL user `q2aUser` with the password `password`, restricted to the local host.*

> [!IMPORTANT]  
> Ensure you use a secure password.

- **Check existing databases:**

```sql
SHOW DATABASES;
```
*Lists all databases on the MySQL server to ensure there is no conflict with the new database name.*

- **Create a new Question2Answer database:**

```sql
CREATE DATABASE question2answer;
SHOW DATABASES;
```
*Creates a new database named `question2answer` and lists all databases to confirm its creation.*

- **Grant all privileges to the new user:**

```sql
GRANT ALL PRIVILEGES ON question2answer.* TO 'q2aUser'@'localhost';
FLUSH PRIVILEGES;
exit
```
*Grants full privileges to `q2aUser` on the `question2answer` database and applies the changes with `FLUSH PRIVILEGES`.*

## 4. Import Database (Optional)

If you are restoring from a production environment backup, use the following commands to import the database:

```bash
USE question2answer;
source /home/azureuser/Downloads/oldbackup.sql
```

## 5. Configure Question2Answer

- **Edit Configuration File**: Set up the qa-config.php file to connect Question2Answer to your MySQL database.

If you're using a new installation of Question2Answer, start by copying the sample configuration file to create a working configuration file. If you're restoring from a backup, the qa-config.php file should already exist; simply edit it as needed.

**For a New Installation:**

```bash
sudo cp qa-config-example.php qa-config.php
```
**Edit the Configuration File:**

```bash
sudo nano qa-config.php
```
In the qa-config.php file, update the following settings with your database information:

- **Database Name:** Replace 'your-database-name' with the name of the database you created.
- **Database User:** Replace 'your-database-username' with the username.
- **Database Password:** Replace 'your-database-password' with the password.

Ensure these settings match the credentials you configured during the MySQL setup. Save and close the file when you're done.

## 6. Final Checks and Testing

After completing these steps, navigate to `http://<yourPublicIPAddress>` in a web browser to access your Question2Answer installation. If restored from a backup, the application should display the restored version.
