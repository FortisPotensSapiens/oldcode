import React, { useState } from 'react';
import { Box, Typography } from "@mui/material";
import { styles } from './style'
import Button from "@mui/material/Button";
import Image from "next/image";
import xIcon from '../../assets/images/svg/xIcon.png';
import { extractNumbers } from '@/helpers/numberValidator'
import { handleAction } from "next/dist/server/app-render/action-handler";
import axios from "axios";
type Props = {
    onPress: (cod: boolean) => React.MouseEventHandler<HTMLButtonElement>,
    id: string,
    paymentSystemId: string,
    paymentNetworkId: string,
};

const TopUpModal: React.FC<Props> = ({ active, onPress, id, paymentSystemId, paymentNetworkId }) => {
    const [money, setMoney] = useState(0)
    const [isValid, setIsValid] = useState(true)
    const handleTransaction = () => {
        setIsValid(money >= 10 && money <= 50000)
        if (!(money >= 10 && money <= 50000))
            return;
        const config = { headers: { Authorization: `Bearer ${localStorage.getItem('accessToken')}` }, }
        const body = {
            accountId: id,
            amount: Number(money),
            paymentNetworkId,
            paymentSystemId,
        }
        axios.post(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/transactions/accruals`, body, config)
            .then((res) => {
                if (res.status === 200) {
                    const transactionId = res.data?.accountTransaction?.id
                    axios.post(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/cryptocloud/invoicies?transactionId=${transactionId}`, {}, config)
                        .then(res => {
                            if (res.status === 200) {
                                window.location.href = res.data.pay_url
                            } else {
                                window.location.href = '/transaction-fail'
                            }

                        })
                }
            })
            .catch(error => window.location.href = '/transaction-fail')
    }
    return (
        <Box sx={styles.box} className={active && 'active'}>
            <Box sx={styles.wrapper}>
                <Box sx={styles.closeButton} onClick={() => onPress(false)}>
                    <Image src={xIcon} />
                </Box>
                <Typography sx={styles.title}>Внесение средств</Typography>
                <Typography sx={styles.text}>
                    Пожалуйста, отправляйте точную сумму к получению с учётом всех комиссий бирж и кошельков,
                    которые вы используете для оплаты.
                </Typography>
                <Box sx={styles.inputWrapper}>
                    <Box>
                        <Typography style={styles.summery}>Сумма</Typography>
                        <Box display={'flex'}>
                            $ <input type="text"
                                style={isValid ? styles.moneyInput : styles.invalidInput}
                                value={money}
                                onChange={(e) => {
                                    const n = extractNumbers(e.target.value)
                                    setMoney(n)
                                    setIsValid(n >= 10 && n <= 50000)
                                }} />
                        </Box>
                    </Box>
                    <Box>
                        <Typography sx={styles.maxPrice}>Макс: <b>50 000 USDT</b></Typography>
                        <Typography sx={styles.maxPrice}>Мин: 10 USDT</Typography>
                    </Box>
                </Box>
                <Box display={'flex'} justifyContent={'space-between'} mt={'67px'} width={'100%'} sx={{
                    '@media only screen and (max-width: 500px)': {
                        flexDirection:'column-reverse',
                        alignItems:'center',
                        marginTop:'27px',
                        gap:'12px',
                        '& button':{
                            width:'100%',
                            height:'52px',
                            fontSize:'12px',
                            fontWeight:'700'
                        }
                    }
                }}>
                    <Button sx={styles.cancelButton} onClick={() => { onPress(false) }}>Отменить</Button>
                    <Button sx={isValid ? styles.continueButton : styles.cancelButton} onClick={handleTransaction}>Продолжить</Button>
                </Box>
            </Box>
        </Box>
    );
};

export default TopUpModal;