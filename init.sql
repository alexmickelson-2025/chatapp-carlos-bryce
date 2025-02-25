

CREATE TABLE message (
    messageId UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    clientId UUID DEFAULT gen_random_uuid(),
    author VARCHAR(255) NOT NULL,
    content VARCHAR(255) NOT NULL,
    creationTime VARCHAR(255),
    clockCounter INT NOT NULL,
    imagePath VARCHAR(255)
);

CREATE TABLE imageapi (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    imagePath VARCHAR(255) NOT NULL,
    apiPort INT NOT NULL
)