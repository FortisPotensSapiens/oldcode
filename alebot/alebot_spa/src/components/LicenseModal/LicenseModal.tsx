import React, {useState} from 'react';
import {Alert, Box, Stack, Typography} from "@mui/material";
import {styles} from '../TopUpModal/style'
import Button from "@mui/material/Button";
import Image from "next/image";
import xIcon from '../../assets/images/svg/xIcon.png';
import {setAccountForLicense} from "@/service/licenses";

type Props = {
    onPress: (cod:boolean)=>React.MouseEventHandler<HTMLButtonElement>,
    setShowAlert: (cod:boolean)=>React.MouseEventHandler<HTMLButtonElement>,
    id:string,
};


const LicenseModal:React.FC<Props> = ({active,onPress,id, onNumberChanged}) => {
    const [tradingAccount,setTradingAccount] = useState('')

    const handleSubmit = () => {
        setAccountForLicense(id,tradingAccount)
            .then(res => {
                console.log(res);
                if(res.status === 200){
                    onPress(false)
                    onNumberChanged()
                }
            })
    }
    return (
        <Box sx={styles.box} className={active && 'active'}>
            <Box sx={styles.wrapper}>
                <Box sx={styles.closeButton} onClick={()=>onPress(false)}>
                    <Image src={xIcon} />
                </Box>
                <Typography sx={styles.title}>Изменение номер счета </Typography>
                <Typography sx={styles.text}>
                    Пожалуйста, отправляйте новый номер счёта для изменение .
                </Typography>
                <Box sx={styles.inputWrapper}>
                    <Box>
                        <Typography style={styles.summery}>счёт</Typography>
                        <Box display={'flex'}>
                            <input type="text"
                                     style={styles.moneyInput}
                                     value={tradingAccount}
                                     placeholder='30038333'
                                     onChange={(e)=>{
                                         setTradingAccount(e.target.value)
                                     }}/>
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
                    <Button sx={styles.cancelButton} onClick={()=>{onPress(false)}}>Отменить</Button>
                    <Button sx={styles.continueButton} onClick={handleSubmit}>Продолжить</Button>
                </Box>
            </Box>
        </Box>
    );
};

export default LicenseModal;