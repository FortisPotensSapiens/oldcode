import React, {useEffect, useState} from 'react';
import {Box, Typography} from "@mui/material";
import {styles} from './style'
import Button from "@mui/material/Button";
import Image from "next/image";
import xIcon from '../../assets/images/svg/xIcon.png';
import api from "@/service/api";
type Props = {
    onPress: (cod:boolean)=>React.MouseEventHandler<HTMLButtonElement>,
    setSuccess: (cod:string)=>React.MouseEventHandler<HTMLButtonElement>,
    setErrorText: (cod:string)=>React.MouseEventHandler<HTMLButtonElement>,
    email:string,
    telegram:string,
    fullName:string,
    phoneNumber:string,
    photo:string
};

const EmailConfirmDodal:React.FC<Props> = ({active, onPress,email,setSuccess,telegram,fullName,setErrorText,phoneNumber,photo}) => {
    const [code,setCode] = useState('')
    const [remainingTime, setRemainingTime] = useState<number>(60);

    useEffect(() => {
        const timerInterval = setInterval(() => {
            if (remainingTime > 0) {
                setRemainingTime((prevTime) => prevTime - 1);
            } else {
                clearInterval(timerInterval);
            }
        }, 1000);
        return () => {
            clearInterval(timerInterval);
        };
    }, [remainingTime]);

    useEffect(()=>{
        setRemainingTime(60)
    },[active])

    const resendEmail = async ()=>{
        await api.get('/api/v1/sso/users/my-user-info')
            .then(res => {
                const body = {
                    userId:res?.data?.id,
                    newEmail:email,
                }
                api.post('/api/v1/sso/send-update-email-code-to-email',body)
                    .then(res => {
                        setRemainingTime(60)
                    })
                    .catch(e => {
                        setErrorText(e.response?.data?.Message);
                        setSuccess('error')
                        onPress(false)
                    })
            })
    }

    const handleSubmit = async ()=>{
        const body = {
          email,
          code,
          telegram,
          fullName,
          phoneNumber,
          photo,
        }
        await api
            .put(`/api/v1/sso/update-my-user-info`,body)
            .then((res) => {
                setSuccess('done')
                onPress(false)
            })
            .catch((e) => {
                setErrorText(e.response?.data?.Message);
                setSuccess('error')
                onPress(false)
            });
    }

    return (
        <Box sx={styles.box} className={active && 'active'}>
            <Box sx={styles.wrapper}>
                <Box sx={styles.closeButton} onClick={()=>onPress(false)}>
                    <Image src={xIcon}  alt={''}/>
                </Box>
                <Typography sx={styles.title}>Укажите код подтверждения email </Typography>
                <Typography sx={styles.text}>
                    Мы отправили вам на почту код подтверждения для смены email на новый нужно указать код что пришел на нее.
                </Typography>
                <Box sx={styles.inputWrapper}>
                    <Box>
                        <Typography style={styles.summery}>Код подтверждения</Typography>
                        <Box display={'flex'}>
                             <input type="text"
                                    value={code}
                                    placeholder='111111'
                                    onChange={e => setCode(e.target.value)}
                                    style={styles.moneyInput}/>
                        </Box>
                    </Box>
                </Box>
                <Box display={'flex'} justifyContent={'space-between'} mt={'67px'} width={'100%'}>
                    <Button sx={styles.cancelButton} onClick={()=>{remainingTime === 0 && resendEmail()}}>
                        Прислать повторно
                        {remainingTime !== 0 && <span>{remainingTime} секунд</span>}
                    </Button>
                    <Button sx={styles.continueButton} onClick={handleSubmit}>Далее</Button>
                </Box>
            </Box>
        </Box>
    );
};

export default EmailConfirmDodal;