import React, { useState } from 'react';
import {
    Box,
    FormControl,
    Input,
    InputLabel,
    MenuItem,
    Modal,
    Select,
    SelectChangeEvent,
    Typography
} from "@mui/material";

import { styles } from '../TopUpModal/style'
import Button from "@mui/material/Button";
import Image from "next/image";
import xIcon from '../../assets/images/svg/xIcon.png';
import { extractNumbers } from '@/helpers/numberValidator'
import api from "@/service/api";
type Props = {
    handleClose: any,
    id: string,
    paymentSystemId: string,
    paymentNetworkId: string,
};

const CashOutModal: React.FC<Props> = ({ active,handleClose, id, paymentSystemId, paymentNetworkId }) => {

    const [money, setMoney] = useState(0)
    const [walletNumber, setWalletNumber] = useState('')
    const [isMoneyValid, setIsMoneyValid] = useState(true)
    const [isWalletNumberValid, setIsWalletNumberValid] = useState(true)


    const handleTransaction = () => {
    setIsMoneyValid((money >= 10 && money <= 50000))
    setIsWalletNumberValid((walletNumber ? true : false))
    if (!((money >= 10 && money <= 50000) && (walletNumber ? true : false)))
        return;
    const config = { headers: { Authorization: `Bearer ${localStorage.getItem('accessToken')}` } }
    const body = {
        accountId: id,
        amount: Number(money),
        paymentNetworkId,
        debitCryptoWalletAddress: walletNumber
    }
    api.post('/api/v1/wallet/transactions/debitings', body, config)
        .then(res => {
            if (res.status === 200) {
                window.location.reload()
            }
        })
        .catch(error => window.location.href = '/transaction-fail')
    setIsMoneyValid((money >= 10 && money <= 50000))
    setIsWalletNumberValid((walletNumber ? true : false))
    if (!((money >= 10 && money <= 50000) && (walletNumber ? true : false)))
        return;
  
    api.post('/api/v1/wallet/transactions/debitings', body, config)
        .then(res => {
            if (res.status === 200) {
                window.location.reload()
            }
        })
        .catch(error => window.location.href = '/transaction-fail')
    }

  

    const [currency, setCurrency] = React.useState(10);
    const [network, setNetwork] = React.useState(10);

    const handleChange = (event: SelectChangeEvent) => {
        setCurrency(event.target.value as string);
    };

    const handleChangeNetwork = (event: SelectChangeEvent) => {
        setNetwork(event.target.value as string);
    };
    const style = {
        position: 'absolute' as 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: 400,
        bgcolor: 'background.paper',
        border: '2px solid #000',
        boxShadow: 24,
        p: 4,
    };
    return (
        <>
            <div>
                <Modal
                    open={active}
                    onClose={handleClose}
                    aria-labelledby="modal-modal-title"
                    aria-describedby="modal-modal-description"
                >
                    <Box sx={styles.boxModalMui}>
                        <Box sx={styles.wrapper}>
                            <Box sx={styles.closeButton} onClick={handleClose}>
                                <Image src={xIcon} />
                            </Box>
                            <Typography sx={styles.title}>Запрос  заявки на вывод</Typography>
                            <Box mt={3} width={'100%'}>
                                <Box sx={{
                                    width: '100%',
                                    '& .selectMUI':{
                                        margin: '20px 0',
                                        width: '100%',
                                    }
                                }}>
                                    <Typography><b>Выберите криптовалюту</b></Typography>
                                    <Select
                                        labelId="demo-simple-select-label"
                                        id="demo-simple-select"
                                        value={currency}
                                        className={'selectMUI'}
                                        onChange={handleChange}
                                        sx={{
                                            position:'relative !important'
                                        }}
                                    >
                                        <MenuItem value={10}>USDT</MenuItem>
                                    </Select>
                                    <Typography><b>Сеть</b></Typography>
                                    <Select
                                        labelId="demo-simple-select-label"
                                        id="demo-simple-select"
                                        value={network}
                                        className={'selectMUI'}
                                        onChange={handleChangeNetwork}
                                        sx={{
                                            position:'relative !important'
                                        }}
                                    >
                                        <MenuItem value={10}>TRC20</MenuItem>
                                    </Select>
                                </Box>
                            </Box>
                            <Box sx={styles.inputWrapper} style={{ marginTop: '10px' }}>
                                <Box>
                                    <Typography sx={styles.summery}>Сумма</Typography>
                                    <Box display={'flex'} sx={{
                                        '& input':{
                                            border: 'none',
                                        }
                                    }}>
                                        <input type="text"
                                               style={(isMoneyValid ? styles.moneyInput : styles.invalidInput)}
                                               value={money}
                                               onChange={(e) => {
                                                   const n = extractNumbers(e.target.value)
                                                   setIsMoneyValid((n >= 10 && n <= 50000))
                                                   setMoney(n)
                                               }}/>
                                    </Box>

                                </Box>
                                <Box>
                                    <Typography sx={styles.maxPrice}>Макс: <b>50 000 USDT</b></Typography>
                                    <Typography sx={styles.maxPrice}>Мин: 10 USDT</Typography>
                                </Box>
                            </Box>
                            <Box sx={styles.inputWrapper} style={{ marginTop: '10px' }}>
                                <Box sx={{
                                    '& input':{
                                        border: 'none',
                                    }
                                }}>
                                    <Typography sx={styles.summery}>Адрес вашего кошелька</Typography>
                                    <Box display={'flex'}>
                                        <input type="text"
                                               style={isWalletNumberValid ? styles.moneyInput : styles.invalidInput}
                                               value={walletNumber}
                                               placeholder='TY9h7s4aQ2hX4N8b4C1Jv2L3pW4ZmR7sT1'
                                               onChange={(e) => {
                                                   setIsWalletNumberValid((e.target.value ? true : false))
                                                   setWalletNumber(e.target.value)
                                               }} />
                                    </Box>
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
                                <Button sx={styles.cancelButton} onClick={handleClose}>Отменить</Button>
                                <Button sx={isMoneyValid && isWalletNumberValid ? styles.continueButton : styles.cancelButton} onClick={handleTransaction}>Продолжить</Button>
                            </Box>
                        </Box>
                    </Box>
                </Modal>
            </div>
        </>

    );


};

export default CashOutModal;