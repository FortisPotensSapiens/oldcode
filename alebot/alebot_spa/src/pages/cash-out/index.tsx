import React, { useEffect, useState } from 'react';
import { Box } from "@mui/material";
import SectionTitle from "@/components/SectionTitle/SectionTitle";
import TransactionsCashOutTable from "@/components/TransactionsCashOutTable/TransactionsCashOutTable";
import axios from "axios";
import Button from "@mui/material/Button";
import { styles } from "@/components/BalanseCard/style";
import CashOutModal from "../../components/CashOutModal/CashoutModal";

const CashOut = () => {
    const [currencies, setCurrencies] = useState()
    const [accounts, setAccounts] = useState([])
    const [paymentSystems, setPaymentSystems] = useState([])
    const [paymentNetworks, setPaymentNetworks] = useState([])
    const [transactions, setTransactions] = useState([])
    const [showModal, setSHowModal] = useState(false)
    useEffect(() => {
        const config = { headers: { Authorization: `Bearer ${localStorage.getItem('accessToken')}` } }

        axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/currencies`)
            .then(res => {
                if (res.status === 200) {
                    setCurrencies(res.data)
                    axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/accounts`, config)
                        .then(res => {
                            if (res.data.length === 0) {
                                axios.post(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/accounts?currencyId=${currencies[0].id}`, {}, config)
                                    .then(res => {
                                        res.status === 200 && setAccounts([res.data])
                                    })
                                    .catch(error => console.log(error))
                            } else {
                                setAccounts(res.data)
                            }
                        })
                        .catch(error => console.log(error))
                }
            })
            .catch(error => console.log(error))

        axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/payment-systems`, config)
            .then(res => { setPaymentSystems(res.data) })
            .catch(error => console.log(error))

        axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/payment-networks`, config)
            .then(res => { setPaymentNetworks(res.data) })
            .catch(error => console.log(error))
    }, [])


    useEffect(() => {
        const config = { headers: { Authorization: `Bearer ${localStorage.getItem('accessToken')}` } }
        if (accounts[0]?.id)
            axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/account/${accounts[0]?.id}/transactions`, config)
                .then(res => {
                    if (res.status === 200) {
                        const data = res.data.filter(e => e.accountTransaction.operationType === "Debiting")
                        setTransactions(data)
                    }
                })
                .catch(error => console.log(error))
    }, [accounts])

    const handleOpen = () => setSHowModal(true);
    const handleClose = () => setSHowModal(false);

    return (
        <Box width={'100%'}>
            <CashOutModal
                id={accounts[0]?.id}
                paymentSystemId={paymentSystems[0]?.id}
                paymentNetworkId={paymentNetworks[0]?.id}
                active={showModal}
                handleClose={handleClose}
                />
            <Box sx={{
                display: 'flex', marginTop: '39px', '@media only screen and (max-width: 640px)': {
                    flexDirection: 'column-reverse'
                }
            }}>
                <TransactionsCashOutTable
                    accounts={accounts}
                    transactions={[]} />
                <Button sx={styles.applyButton} onClick={handleOpen}>
                    <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 20 20" fill="none">
                        <path fillRule="evenodd" clipRule="evenodd" d="M9.44 0H10.56C15.7736 0 20 4.22643 20 9.44V10.56C20 15.7736 15.7736 20 10.56 20H9.44C4.22643 20 0 15.7736 0 10.56V9.44C0 4.22643 4.22643 0 9.44 0ZM10.56 18.5C14.9315 18.4673 18.4673 14.9315 18.5 10.56V9.44C18.4673 5.06846 14.9315 1.53267 10.56 1.5H9.44C5.06846 1.53267 1.53267 5.06846 1.5 9.44V10.56C1.53267 14.9315 5.06846 18.4673 9.44 18.5H10.56Z" fill="white" />
                        <path d="M14 9.25H10.75V6C10.75 5.58579 10.4142 5.25 10 5.25C9.58579 5.25 9.25 5.58579 9.25 6V9.25H6C5.58579 9.25 5.25 9.58579 5.25 10C5.25 10.4142 5.58579 10.75 6 10.75H9.25V14C9.25 14.4142 9.58579 14.75 10 14.75C10.4142 14.75 10.75 14.4142 10.75 14V10.75H14C14.4142 10.75 14.75 10.4142 14.75 10C14.75 9.58579 14.4142 9.25 14 9.25Z" fill="white" />
                    </svg>
                    Подать заявку
                </Button>
            </Box>
        </Box>
    );
};

export default CashOut;