import React, { useEffect, useState } from "react";
import Box from "@mui/material/Box";
import { styles } from './style'
import TransactionsTable from "@/components/Transactions/TransactionsTable";
import BalanceCard from "@/components/BalanseCard/BalanceCard";

import axios from "axios";

export default function StarredPage() {
    const [currencies, setCurrencies] = useState()
    const [accounts, setAccounts] = useState([])
    const [paymentSystems, setPaymentSystems] = useState([])
    const [paymentNetworks, setPaymentNetworks] = useState([])
    const [transactions, setTransactions] = useState([])

    useEffect(() => {
        const config = { headers: { Authorization: `Bearer ${localStorage.getItem('accessToken')}` } }

        axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/currencies`)
            .then(res => {
                if (res.status === 200) {
                    const CurId = res.data[0].id
                    axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/accounts`, config)
                        .then(res => {
                            if (res.data.length === 0) {
                                axios.post(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/accounts?currencyId=${CurId}`, {}, config)
                                    .then(res => {
                                        res.status === 200 && setAccounts([res.data])
                                    })
                                    .catch(error => console.log(error))
                            } else {
                                setAccounts(res.data)
                            }
                        })
                        .catch(error => console.log(error))
                    setCurrencies(res.data)
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
                        let data = res.data.filter(e => e.accountTransaction.state === 'Completed')
                        setTransactions(data)
                    }
                })
                .catch(error => console.log(error))
    }, [accounts])
    return (
        <Box width={'100%'}>
            <Box sx={styles.container}>
                <TransactionsTable
                    accounts={accounts}
                    transactions={[]} />
                <BalanceCard
                    paymentNetworks={paymentNetworks}
                    paymentSystems={paymentSystems}
                    transactions={transactions}
                    accounts={accounts} />
            </Box>
        </Box>
    );
}
