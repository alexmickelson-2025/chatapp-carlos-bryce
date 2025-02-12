

CREATE TABLE message (
    id SERIAL PRIMARY KEY,       
    author VARCHAR(255) NOT NULL,
    content VARCHAR(255) NOT NULL,
    creationTime VARCHAR(255)              
);