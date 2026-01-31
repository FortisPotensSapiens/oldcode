import React from 'react';
import {styles} from './style'
import {Box, Button} from "@mui/material";
const TransactionFail = () => {
    return (
        <Box sx={styles.container}>
            <Box sx={styles.content}>
                <h1>Транзакция не удалась</h1>
                <p>К сожалению, ваш платеж не был успешным.</p>
                <p>Пожалуйста, попробуйте еще раз или обратитесь в службу поддержки за помощью.</p>
                <Button href={'/wallet'} sx={styles.button}>Вернуться к транзакциям</Button>
            </Box>
        </Box>
    );
};

export default TransactionFail;