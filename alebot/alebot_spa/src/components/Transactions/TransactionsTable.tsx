import React, { useEffect, useState } from 'react';
import { Box, Typography } from "@mui/material";
import 'react-date-range/dist/styles.css'
import 'react-date-range/dist/theme/default.css'
import { styles } from './style'
import SecondTitle from "@/components/SecondTitle/SecondTitle";
import { DateRange } from "react-date-range";
import Image from "next/image";
const datePickerIcon = require('../../assets/images/svg/datePickerIcon.png');
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import moment from "moment";
import axios from "axios";
import { ru } from "date-fns/locale";

const arrowUp = require('../../assets/images/svg/arrowup.png');
const arrowDown = require('../../assets/images/svg/arrowDown.png');


type Props = {
    transactions: any,
    accounts: any,
}
const TransactionsTable: React.FC<Props> = ({ transactions, accounts }) => {
    const [showDatePicker, setShowDatePicker] = useState(false)

    const [rows, setRows] = useState([])
    const [date, setDate] = useState([
        {
            startDate: new Date(),
            endDate: new Date(),
            key: 'selection'
        }
    ])
    useEffect(() => {
        let yearPlus = new Date()
        yearPlus.setFullYear(yearPlus.getFullYear() - 1)
        yearPlus.setDate(yearPlus.getDate() - 1);
        setDate([{
            startDate: yearPlus,
            endDate: new Date(),
            key: 'selection'
        }])
    }, [])
    useEffect(() => {
        setRows(transactions.map(e => e?.accountTransaction))
    }, [transactions])
    useEffect(() => {
        let startFrom = date[0].startDate
        let ednTo = date[0].endDate
        startFrom.setHours(0);
        startFrom.setMinutes(0);
        startFrom.setSeconds(0);
        startFrom.setMilliseconds(0);
        ednTo.setHours(23);
        ednTo.setMinutes(59);
        ednTo.setSeconds(59);
        ednTo.setMilliseconds(0);
        const config = { headers: { Authorization: `Bearer ${localStorage.getItem('accessToken')}` } }
        if (accounts[0]?.id)
            axios.get(`${process.env.NEXT_PUBLIC_URL}/api/v1/wallet/account/${accounts[0]?.id}/transactions?dateFrom=${startFrom.toISOString()}&dateTo=${ednTo.toISOString()}`, config)
                .then(res => {
                    if (res.status === 200) {
                        let data = res.data.filter(e => e.accountTransaction.state === 'Completed')
                        setRows(data.map(e => e?.accountTransaction))
                    }
                })
                .catch(error => console.log(error))
    }, [date, transactions, accounts])
    return (
        <Box sx={styles.box}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between',
                '@media only screen and (max-width: 640px)': {
                    flexDirection: 'column',
                    gap:'10px',
                    marginBottom: '10px'
                }}}>
                <SecondTitle>Транзакции</SecondTitle>
                <Box>
                    <Box sx={styles.dateShower} onClick={() => setShowDatePicker(!showDatePicker)}>
                        <Image src={datePickerIcon} style={{ marginRight: '15px' }} />
                        {date[0]?.startDate.toLocaleDateString()} - {date[0]?.endDate.toLocaleDateString()}
                        {showDatePicker && <Box sx={styles.datepickerWrapper}>
                            <DateRange
                                editableDateInputs={true}
                                onChange={(item: any) => setDate([item.selection])}
                                moveRangeOnFirstSelection={false}
                                ranges={date}
                                locale={ru}
                            />
                        </Box>}
                    </Box>
                </Box>
            </Box>
            {rows.length === 0 ? <Typography sx={styles.noData}>Транзакции не найдены</Typography> :
                <Table sx={styles.table} aria-label="caption table">
                    <TableHead sx={{
                        '@media only screen and (max-width: 640px)': {
                           display: "none",
                        }
                    }}>
                        <TableRow>
                            <TableCell>Тип операции</TableCell>
                            <TableCell align="left">Дата</TableCell>
                            <TableCell align="left">Описание операции</TableCell>
                            <TableCell align="right">Сумма</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {rows.map((row) => (
                            <TableRow key={row.id} sx={{
                                '@media only screen and (max-width: 640px)': {
                                    display:'flex',
                                    flexDirection:'column',
                                    width:'100%',
                                    border:'1px solid #D9D9D9',
                                    marginBottom:'22px',
                                    borderRadius:'20px',
                                    overflow:'hidden',
                                }
                            }}>
                                <TableCell component="th" scope="row">
                                    <Typography sx={{
                                        fontSize:'10px',
                                        fontWeight:'600',
                                        marginBottom:'5px',
                                        color:'#000',
                                        display:'none',
                                        '@media only screen and (max-width: 640px)': {
                                            display:'block',
                                        }
                                    }}>Тип операции</Typography>
                                    {row.operationType !== 'Accrual' ? <Image src={arrowDown} /> : <Image src={arrowUp} />}

                                </TableCell>
                                <TableCell align="left">
                                    <Typography sx={{
                                        fontSize:'10px',
                                        fontWeight:'600',
                                        marginBottom:'5px',
                                        color:'#000',
                                        display:'none',
                                        '@media only screen and (max-width: 640px)': {
                                            display:'block',
                                        }
                                    }}>Дата</Typography>
                                    {moment(new Date(row.created).toDateString()).format('DD.MM.Y')}
                                </TableCell>
                                <TableCell align="left">
                                    <Typography sx={{
                                        fontSize:'10px',
                                        fontWeight:'600',
                                        marginBottom:'5px',
                                        color:'#000',
                                        display:'none',
                                        '@media only screen and (max-width: 640px)': {
                                            display:'block',
                                        }
                                    }}>Описание операции</Typography>
                                    {row.operationDescription}
                                </TableCell>
                                <TableCell sx={{
                                    textAlign:'right',
                                    '@media only screen and (max-width: 640px)': {
                                        textAlign:'left'
                                    }
                                }}>
                                    <Typography sx={{
                                        fontSize:'10px',
                                        fontWeight:'600',
                                        marginBottom:'5px',
                                        color:'#000',
                                        display:'none',
                                        '@media only screen and (max-width: 640px)': {
                                            display:'block',
                                        }
                                    }}>Сумма</Typography>
                                    <b>$ {row.amount}</b>
                                </TableCell>
                            </TableRow>))}
                    </TableBody>
                </Table>}
        </Box>
    );
};

export default TransactionsTable;
