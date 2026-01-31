import React, {useEffect, useState} from 'react';
import {Box, Typography} from "@mui/material";
import {styles} from '../TopUpModal/style'
import Button from "@mui/material/Button";
import Image from "next/image";
import xIcon from '../../assets/images/svg/xIcon.png';
import {extractNumbers} from '@/helpers/numberValidator'
import api from "@/service/api";
type Props = {
    onPress: (cod:boolean)=>React.MouseEventHandler<HTMLButtonElement>,
    id:string,
};

const ProductModal:React.FC<Props> = ({active,onPress,id}) => {
    const [check,setCheck] = useState('')
    const handleTransaction = () => {
        let body = {
            merchId:id,
            tradingAccount:check,
        }
        api.post(`/api/v1/shop/orders`,body)
            .then(res => {
                if(res.status === 200){
                    api.post(`/api/v1/shop/cryptocloud/invoicies?orderId=${res.data}`)
                        .then(res => {
                            window.location.href =res.data.pay_url;
                            onPress(false)
                        })
                        .catch(e => console.log(e))
                }
            })
            .catch(e => console.log(e))
    }
    return (
        <Box sx={styles.box} className={active && 'active'}>
            <Box sx={styles.wrapper}>
                <Box sx={styles.closeButton} onClick={()=>onPress(false)}>
                    <Image src={xIcon} />
                </Box>
                <Typography sx={styles.title}>Укажите номер счета
                            для торговли трейдером</Typography>
                <Typography textAlign={'center'} fontSize={'10px'} mt={2}>
                    Вы не сможете использовать бота пока не укажете номер брокерского счета.
                    Вы можете его указать позже на странице Лицензии для выбранной лицензии.
                </Typography>
                <Box sx={styles.inputWrapper} style={{marginTop:'10px'}}>
                    <Box width={'100%'}>
                        <Typography sx={styles.summery}>Брокерский счёт</Typography>
                        <Box display={'flex'}>
                            <input type="text"
                                   style={styles.moneyInput}
                                   value={check}
                                   onChange={(e)=> setCheck(e.target.value)}/>
                        </Box>
                    </Box>
                </Box>
                <Box display={'flex'} justifyContent={'space-between'} mt={'67px'} width={'100%'}>
                    <Button sx={styles.cancelButton} onClick={()=>{onPress(false)}}>Отменить</Button>
                    <Button sx={styles.continueButton} onClick={handleTransaction}>Продолжить</Button>
                </Box>
            </Box>
        </Box>
    );
};

export default ProductModal;