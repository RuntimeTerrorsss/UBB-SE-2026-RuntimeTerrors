USE MyDb;
GO

-- Clear existing data (in correct order to respect foreign keys)
    DELETE FROM job_skills;
    DELETE FROM skills;
    DELETE FROM applicants;
    DELETE FROM users;
    DELETE FROM jobs;
    DELETE FROM companies;

-- Seed Companies
    INSERT INTO companies (company_id, company_name, about_us, profile_picture_url, logo_picture_url, location, email) 
    VALUES (1, 'Tech Corp', 'IT Company', 'pfp.png', 'logo.png', 'Local', 'contact@tech.com');

-- Seed Jobs
    INSERT INTO jobs (job_id, company_id, job_title, industry_field, job_type, experience_level, job_description, job_location, available_positions)
    VALUES (1, 1, 'Software Engineer', 'IT', 'Full Time', 'Junior', 'Write code', 'Local', 5);

-- Seed Skills
    SET IDENTITY_INSERT skills ON;
    INSERT INTO skills (skill_id, skill_name) VALUES (1, 'c#');
    INSERT INTO skills (skill_id, skill_name) VALUES (2, 'sql');
    INSERT INTO skills (skill_id, skill_name) VALUES (3, '.net');
    SET IDENTITY_INSERT skills OFF;

-- Seed Job Skills (Link jobs to skills)
    INSERT INTO job_skills (job_id, skill_id, required_percentage) VALUES (1, 1, 80);
    INSERT INTO job_skills (job_id, skill_id, required_percentage) VALUES (1, 2, 70);
    INSERT INTO job_skills (job_id, skill_id, required_percentage) VALUES (1, 3, 60);

-- Seed Users
    SET IDENTITY_INSERT users ON;
    INSERT INTO users (user_id, name, email) VALUES (1, 'Cipicao ', 'florea.ciprianm@gmail.com');
    INSERT INTO users (user_id, name, email) VALUES (2, 'Bobby', 'bobbyb@test.com');
    SET IDENTITY_INSERT users OFF;

-- Seed Applicants
    INSERT INTO applicants (applicant_id, job_id, cv_file_url, application_status, applied_at, user_id)
    VALUES (101, 1, 'C:\dummy_cv1.xml', 'Pending', GETDATE(), 1);
    
    INSERT INTO applicants (applicant_id, job_id, cv_file_url, application_status, applied_at, user_id, app_test_grade, cv_grade)
    VALUES (102, 1, 'C:\dummy_cv2.xml', 'On Hold', GETDATE(), 2, 7.5, 8.0);
