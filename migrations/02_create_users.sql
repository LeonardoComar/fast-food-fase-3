CREATE USER 'fastfood_user'@'%' IDENTIFIED BY 'Mudar123!';
GRANT ALL PRIVILEGES ON fastfood.* TO 'fastfood_user'@'%';

-- Flush privileges to apply changes
FLUSH PRIVILEGES;
