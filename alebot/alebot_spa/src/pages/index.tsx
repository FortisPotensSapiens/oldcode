import * as React from 'react';
import Box from '@mui/material/Box';
import { styles } from './style'
import { Alert, Stack, Typography } from "@mui/material";
import Button from "@mui/material/Button";
import Image from "next/image";
import CoursesSlider from "@/components/CoursesSlider/CoursesSlider";
import IncomeCard from "@/components/IcomeCard/IncomeCard";
import { useEffect, useState } from "react";
import api from "@/service/api";
import { copyText } from "@/helpers/copyText";
import TopUpModal from "@/components/TopUpModal/TopUpModal";
import axios from "axios";
import Link from "next/link";
const lineArrowDown = require('../assets/images/svg/lineArrowDown.png');
const lineArrowUp = require('../assets/images/svg/lineArrowUp.png');
const arrowUpWhite = require('../assets/images/svg/arrowUpWhite.png');
const arrowDownWhite = require('../assets/images/svg/arrowDownWhite.png');
const person = require('../assets/images/svg/balance/person.png');
const group = require('../assets/images/svg/balance/group.png');
const share = require('../assets/images/svg/balance/share.png');
const openedfolder = require('../assets/images/svg/balance/openedfolder.png');
const twoperson = require('../assets/images/svg/balance/twoperson.png');

export default function HomePage() {
    const [account, setAccount] = useState({
        id: '',
        fullName: '',
        email: '',
        telegram: '',
        refererEmail: '',
        refererFullName: '',
        refererId: '',
    })
    const [wallet, setWallet] = useState({
        id: '',
        userId: '',
        currencyId: '',
        amount: 0
    })
    const [transactions, setTransactions] = useState([])
    const [topUpAmount, setTopUpAmount] = useState(0)
    const [cashOutAmount, setCashOutAmount] = useState(0)
    const [showAlert, setShowAlert] = useState(false)
    const [coursePath, setCoursePath] = useState('')
    const [paymentSystems, setPaymentSystems] = useState([])
    const [paymentNetworks, setPaymentNetworks] = useState([])
    const [showModal, setSHowModal] = useState(false)
    const [income, setIncome] = useState({
        total: 0,
        products: 0,
    })
    useEffect(() => {
        api.get('/api/v1/sso/users/my-user-info')
            .then(res => {
                setAccount(res.data)
                api.get(`/api/v1/wallet/accounts`)
                    .then(res => {
                        if (res.status === 200) {
                            setWallet(res?.data[0])
                            if (res?.data[0]?.id)
                                api.get(`/api/v1/wallet/account/${res?.data[0]?.id}/transactions`)
                                    .then(res => {
                                        let data = res.data.filter(e => e.accountTransaction.state === 'Completed')
                                        setTransactions(data)
                                    })
                                    .catch(e => console.log(e))
                        }
                    })
                    .catch(e => console.log(e))
            })
            .catch(e => console.log(e))
        api.get('/api/v1/wallet/my-income')
            .then(res => {
                setIncome(res.data)
            })
            .catch(e => console.log(e))

        api.get(`/api/v1/wallet/payment-systems`)
            .then(res => { setPaymentSystems(res.data) })
            .catch(error => console.log(error))

        api.get(`/api/v1/wallet/payment-networks`)
            .then(res => { setPaymentNetworks(res.data) })
            .catch(error => console.log(error))
        setCoursePath(window.location.href)
    }, [])

    useEffect(() => {
        let upAmount = 0
        let outAmount = 0
        transactions.map((e) => {
            if (e.accountTransaction.operationType === 'Accrual') {
                upAmount += e.accountTransaction.amount
            } else {
                outAmount += e.accountTransaction.amount
            }
            setTopUpAmount(upAmount)
            setCashOutAmount(outAmount)
        })
    }, [transactions])
    const loaderProp = ({ src }) => {
        return src;
    }
    return (
        <Box sx={styles.container}>
            {showAlert && <Stack sx={{ width: '400px', position: 'fixed', right: '20px', bottom: '20px' }} spacing={2}>
                <Alert severity="success">Реферальная ссылка скопирована</Alert>
            </Stack>}
            <TopUpModal
                id={wallet?.id}
                paymentSystemId={paymentSystems[0]?.id}
                paymentNetworkId={paymentNetworks[0]?.id}
                active={showModal}
                onPress={setSHowModal} />
            <Box sx={styles.box}>
                <Box sx={styles.balance}>
                    <Box sx={styles.titleBox}>
                        <Typography sx={styles.balanceTitle}>Текущий баланс</Typography>
                        <Box sx={styles.currencySymbol}>$</Box>
                    </Box>
                    <Box sx={styles.moneyAmount}>$ {wallet?.amount}</Box>
                    <Box sx={{
                        display:'grid',
                        gridTemplateColumns:'repeat(2,1fr)',
                        gap:'23px',
                        marginLeft: '37px',
                        marginTop: '25px',
                        paddingRight:'37px'
                    }}>
                        <Button sx={styles.topUpButton} onClick={() => setSHowModal(true)}>
                            <Image src={lineArrowUp} style={{ marginRight: '7px' }} />
                            Пополнить
                        </Button>
                        <Link href='/cash-out'>
                            <Button sx={styles.withdrawButton} >
                                <Image src={lineArrowDown} style={{ marginRight: '7px' }} />
                                Вывести
                            </Button>
                        </Link>
                    </Box>
                    <Box sx={{ display: 'flex', marginLeft: '37px', marginTop: '60px' }}>
                        <Box sx={{ display: 'flex', marginRight: '62px' }}>
                            <Image src={arrowUpWhite} />
                            <Box sx={styles.smallPriceWrapper}>
                                <Typography sx={styles.smallPriceTitle}>Получено</Typography>
                                <Typography sx={styles.smallPrice}>+ $ {topUpAmount}</Typography>
                            </Box>
                        </Box>
                        <Box display={'flex'} >
                            <Image src={arrowDownWhite} />
                            <Box sx={styles.smallPriceWrapper}>
                                <Typography sx={styles.smallPriceTitle}>Выведено</Typography>
                                <Typography sx={styles.smallPrice}>- $ {cashOutAmount}</Typography>
                            </Box>
                        </Box>
                    </Box>
                </Box>
                <Box sx={styles.info}>
                    <Typography sx={styles.balanceTitle}>Информация</Typography>
                    <Box>
                        <Link href={'/settings'}>
                            <Box sx={styles.infoValue}>
                                <Image src={person} alt='person icon' />
                                <Box>
                                    <Typography sx={styles.infoValueTitle}>{account.fullName}</Typography>
                                    <Typography sx={styles.infoValueText}>{account.email}</Typography>
                                </Box>
                            </Box>
                        </Link>
                        <Box sx={styles.infoValue}>
                            <Image src={share} alt='person icon' />
                            <Box>
                                <Typography sx={styles.infoValueTitle}>Реферальная ссылка:</Typography>
                                <a href="#"
                                    style={styles.infoValueText}
                                    onClick={() => {
                                        copyText(`${window.location.origin}/sign-up?referrer=${account.id}`)
                                        setShowAlert(true)
                                        setTimeout(() => { setShowAlert(false) }, 3000)
                                    }}
                                    rel="noopener noreferrer">{account.id}</a>
                            </Box>
                        </Box>
                        <Box sx={styles.infoValue}>
                            <Image src={group} alt='person icon' loader={loaderProp} />
                            <Box>
                                <Typography sx={styles.infoValueTitle}>Телеграм канал</Typography>
                                <a href='https://t.me/Ale_bot1'
                                    style={styles.infoValueText}
                                    target="_blank"
                                    rel="noopener noreferrer">https://t.me/Ale_bot1</a>
                            </Box>
                        </Box>
                        <Link href={coursePath + 'courses'} style={styles.infoValueText}>
                            <Box sx={styles.infoValue}>
                                <Image src={openedfolder} alt='person icon' />
                                <Box>
                                    <Typography sx={styles.infoValueTitle}>База знаний</Typography>
                                </Box>
                            </Box>
                        </Link>
                        {account.refererId && <Box sx={styles.infoValue}>
                            <Image src={twoperson} alt='person icon' />
                            <Box>
                                <Typography sx={styles.infoValueTitle}>Реферер</Typography>
                                <Box sx={{ display: 'flex', alignItems: 'center', gap: '5px' }}>
                                    <Typography sx={styles.infoValueText}>{account.refererFullName}</Typography>
                                    <a href={`mailto: ${account.refererEmail}`}
                                        style={styles.infoValueText}
                                        target="_blank"
                                        rel="noopener noreferrer">{account.refererEmail}</a>
                                </Box>
                            </Box>
                        </Box>}
                    </Box>
                </Box>
            </Box>
            <Box sx={{ ...styles.box, marginTop: '15px' }}>
                <Box sx={{
                    width: '50%', '@media only screen and (max-width: 1200px)': {
                        width: '100%',
                    },
                }}>
                    <CoursesSlider />
                </Box>
                <Box sx={{
                    width: '50%', '@media only screen and (max-width: 1200px)': {
                        width: '100%',
                    },
                }}>
                    <IncomeCard income={income} />
                </Box>
            </Box>
        </Box>
    );
}
