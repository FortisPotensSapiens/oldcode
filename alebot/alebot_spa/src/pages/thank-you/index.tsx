import React from 'react';
import {styles} from './style'
import {Box, Button} from "@mui/material";
const ThankYouPage = () => {
    return (
        <Box sx={styles.container}>
            <Box sx={styles.content}>
                <h1>Спасибо за вашу транзакцию!</h1>
                <p>Ваш платеж прошел успешно.</p>
                <p>Мы ценим ваше дело.</p>
                <Button href={'/wallet'} sx={styles.button}>Вернуться к транзакциям</Button>
            </Box>
        </Box>
    );
};

export default ThankYouPage;