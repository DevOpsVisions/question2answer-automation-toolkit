# Deploying or Restoring Question2Answer on an Azure VM

This guide provides a comprehensive walkthrough for setting up a testing environment on Azure by deploying or restoring Question2Answer on an Ubuntu VM. The instructions cover preparing a LAMP stack environment, configuring essential components, and deploying the Question2Answer application. Follow these steps to ensure a smooth deployment and configuration process for testing purposes.

## 1. Preparing a LAMP Stack Environment

To prepare your environment, refer to the following guide:

[Preparing a LAMP Stack Environment for Hosting](https://github.com/DevOpsVisions/common-workspace-hub/blob/main/docs/technical-guides/prepare_lamp_stack_env.md)

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
mysql -u root -p
```
Enter the root password set during the MySQL installation.

- **Create MySQL User and Database**: Run the following commands to create a new MySQL user and database for Question2Answer. These commands also grant the required privileges to the new user on the newly created database.

```sql
CREATE USER 'q2aUser'@'localhost' IDENTIFIED BY 'password';
```
```sql
CREATE DATABASE question2answer;
```
```sql
GRANT ALL PRIVILEGES ON question2answer.* TO 'q2aUser'@'localhost';
```
```sql
FLUSH PRIVILEGES;
```

## 4. Configure Question2Answer

- **Edit Configuration File**: Copy the example configuration file and edit it to include database and other settings.

```bash
sudo cp qa-config-example.php qa-config.php
```
```bash
sudo nano qa-config.php
```
In `qa-config.php`, provide the database name, user, and password created in the MySQL configuration step.

## 5. Import Database (Optional)

If you are restoring from a production environment backup, use the following commands to import the database:

```bash
USE question2answer;
source /home/azureuser/Downloads/oldbackup.sql
```

## 6. Access Your Question2Answer Installation

After completing these steps, navigate to `http://<yourPublicIPAddress>` in a web browser to access your Question2Answer installation. If restored from a backup, the application should display the restored version.