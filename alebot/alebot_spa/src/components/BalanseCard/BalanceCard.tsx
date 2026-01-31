import React, {useEffect, useState} from 'react';

import {styles} from './style';
import Image from "next/image";
import SecondTitle from "@/components/SecondTitle/SecondTitle";
import Button from "@mui/material/Button";
import TopUpModal from "@/components/TopUpModal/TopUpModal";
import {Box, Typography} from "@mui/material";
const arrowUp = require('../../assets/images/svg/arrowup.png');
const arrowDown = require('../../assets/images/svg/arrowDown.png');
const lineArrowDown = require('../../assets/images/svg/lineArrowDown.png');
const lineArrowUp = require('../../assets/images/svg/lineArrowUp.png');
import { useRouter } from 'next/router'

type Props = {
    paymentNetworks?:any[],
    paymentSystems?:any[],
    accounts?:any[],
    transactions:any[],
}
const BalanceCard:React.FC<Props> = ({paymentNetworks,paymentSystems,accounts,transactions}) => {
    const [showModal, setSHowModal] = useState(false)
    const [topUpAmount, setTopUpAmount] = useState(0)
    const [cashOutAmount, setCashOutAmount] = useState(0)
    const router = useRouter()
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
    return (
        <Box sx={styles.box}>
            <TopUpModal
                id={accounts[0]?.id}
                paymentSystemId={paymentSystems[0]?.id}
                paymentNetworkId={paymentNetworks[0]?.id}
                active={showModal}
                onPress={setSHowModal}/>
            <SecondTitle>Текущий баланс</SecondTitle>
            <Typography sx={styles.price}>${accounts[0]?.amount}</Typography>
            <Box display={'flex'} mt={2}>
                <Box sx={{display:'flex',marginRight:'62px'}}>
                    <Image src={arrowUp} />
                    <Box sx={styles.smallPriceWrapper}>
                        <Typography sx={styles.smallPriceTitle}>Получено</Typography>
                        <Typography sx={styles.smallPrice}>+ ${topUpAmount}</Typography>
                    </Box>
                </Box>
                <Box display={'flex'} >
                    <Image src={arrowDown} />
                    <Box sx={styles.smallPriceWrapper}>
                        <Typography sx={styles.smallPriceTitle}>Выведено</Typography>
                        <Typography sx={styles.smallPrice}>- ${cashOutAmount}</Typography>
                    </Box>
                </Box>
            </Box>
            <Box display={'flex'} sx={{marginTop:'50px'}}>
                <Button sx={styles.withdrawButton} onClick={() => router.push('/cash-out')}>
                    <Image src={lineArrowDown} style={{marginRight:'7px'}}/>
                    Вывести
                </Button>
                <Button sx={styles.topUpButton} onClick={()=>setSHowModal(true)}>
                    <Image src={lineArrowUp} style={{marginRight:'7px'}}/>
                    Пополнить
                </Button>
            </Box>
        </Box>
    )
}
export default BalanceCard;
