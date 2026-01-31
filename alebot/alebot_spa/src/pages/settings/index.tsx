import * as React from "react";
import Container from "@mui/material/Container";
import SectionTitle from "@/components/SectionTitle/SectionTitle";
import { Box, Typography } from "@mui/material";
import { styles } from './style'
import { useEffect, useRef, useState } from "react";
import { getSettings } from "@/service/settings";
import Button from "@mui/material/Button";
import api from "@/service/api";
import downArrow from '../../assets/svg/downArrow.png'
import Image from "next/image";
import EmailConfirmDodal from "@/components/EmailConfirmDodal/EmailConfirmDodal";
import telegramIcon from '../../assets/svg/telegramIcon.png';
import MuiPhoneNumber from "mui-phone-number";

export default function StarredPage() {
    const [imageSrc, setImageSrc] = useState<string | null>(null);
    const [fullName, setFullName] = useState('')
    const [telegram, setTelegram] = useState('')
    const [email, setEmail] = useState('')
    const [phoneNumber, setPhoneNumber] = useState('')
    const [savedEmail, setSavedEmail] = useState('')
    const [success, setSuccess] = useState('process')
    const [errorText, setErrorText] = useState('')
    const [errorField, setErrorField] = useState('')
    const [shoeModal, setShowModal] = useState(false)
    const fileInputRef = useRef<HTMLInputElement | null>(null);

    useEffect(() => {
        getSettings().then((data) => {
            console.log(data, 'data');
            setEmail(data?.email)
            setSavedEmail(data?.email)
            setFullName(data?.fullName)
            setPhoneNumber(data?.phoneNumber)
            setTelegram(data?.telegram)
            setImageSrc(`data:image/png;base64,${data?.photo}`)
        });
    }, [success]);

    const handleUploadClick = () => {
        if (fileInputRef.current) {
            fileInputRef.current.click();
        }
    };

    const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const selectedFile = e.target.files && e.target.files[0];
        if (selectedFile) {
            const reader = new FileReader();
            reader.onloadend = () => {
                const base64String = reader.result as string;
                setImageSrc(base64String);
            };
            reader.readAsDataURL(selectedFile);
        }
    };

    const handleError = (err) => {
        const field = Object.keys(err)[0]
        const text: string = Object.values(err)[0][0]
        setErrorText(text)
        setErrorField(field)

    }

    const handleSubmit = async () => {
        if (email !== savedEmail) {
            await api.get('/api/v1/sso/users/my-user-info')
                .then(res => {
                    const body = {
                        userId: res?.data?.id,
                        newEmail: email,
                    }
                    api.post('/api/v1/sso/send-update-email-code-to-email', body)
                        .then(res => {
                            res.status === 200 && setShowModal(true)
                        })
                        .catch(e => {
                            console.log(e);
                            if (e.response?.data.errors) {
                                handleError(e.response?.data.errors)
                            } else {
                                setSuccess('error')
                                setErrorText(e.response?.data.Message)
                            }
                        })
                })
        } else {
            const body = {
                email,
                fullName,
                telegram,
                phoneNumber,
            }
            await api
                .put(`/api/v1/sso/update-my-user-info`, body)
                .then((res) => res.status === 200 && setSuccess('done'))
                .catch((e) => {
                    handleError(e.response?.data.errors)
                    setSuccess('error')
                });
        }
    }

    return (
        <Container sx={{
            '@media only screen and (max-width: 640px)': {
                marginBottom:'50px',
            },
        }}>
            <EmailConfirmDodal
                onPress={setShowModal}
                setSuccess={setSuccess}
                setErrorText={setErrorText}
                email={email}
                phoneNumber={phoneNumber}
                telegram={telegram}
                fullName={fullName}
                photo={imageSrc?.split(',')[1]}
                active={shoeModal} />
            <SectionTitle>Настройки</SectionTitle>
            <Box sx={styles.box}>
                <Typography sx={styles.title}>Информация
                    {success === 'done' && <span>Ваши изменение успешно сахранени</span>}
                    {success === 'error' && <span className='error'>{errorText}</span>}
                </Typography>
                <Box sx={{
                    '@media only screen and (max-width: 1400px)': {
                        flexDirection: 'column-reverse',
                        gap: '20px',
                    },
                }} display={'flex'} mt={4}>
                    <Box sx={styles.inputContainer}>
                        <Box sx={styles.inputWrapper} className={errorField === 'FullName' && 'error'}>
                            <label>
                                <p>Имя</p>
                                <input type="text" value={fullName} onChange={e => setFullName(e.target.value)} />
                            </label>
                        </Box>
                        <Box sx={styles.inputWrapper} className={errorField === 'Telegram' && 'error'}>
                            <label>
                                <p>Телеграм</p>
                                <input type="text" value={telegram} onChange={e => setTelegram(e.target.value)} />
                            </label>
                            <Image src={telegramIcon} style={styles.telegramIcon} />
                        </Box>
                        <Box sx={styles.inputWrapper} className={errorField === 'NewEmail' && 'error'}>
                            <label>
                                <p>Электронная почта</p>
                                <input type="text" value={email} onChange={e => setEmail(e.target.value)} />
                            </label>
                        </Box>
                        <Box sx={{...styles.inputWrapper,
                            '& .MuiInputBase-root':{
                                marginTop:'-3px',
                            },
                            '& .MuiInputBase-root:before':{
                                display:'none',
                            },
                            '& button.MuiButtonBase-root':{
                                minWidth:'19px !important',
                            }
                        }} className={errorField === 'PhoneNumber' && 'error'}>
                            <label>
                                <p>Номер телефона</p>
                                <MuiPhoneNumber
                                    defaultCountry={'ru'}
                                    name='phoneNumber'
                                    value={phoneNumber}
                                    onChange={e => setPhoneNumber(e)}
                                    autoFormat={false}
                                    inputProps={{ maxLength: 13 }}
                                />
                                {/*<input type="text" value={phoneNumber} onChange={e => setPhoneNumber(e.target.value)} />*/}
                            </label>
                        </Box>
                        <Box display={'flex'} justifyContent={'flex-end'} width={'100%'} gridColumn={'1 / 3'} sx={{
                            '@media only screen and (max-width: 640px)': {
                                justifyContent:'center',
                                paddingBottom:'20px'
                            }
                        }}>
                            <Button sx={styles.submitButton} style={{height:'52px'}} onClick={handleSubmit}>Сохранить</Button>
                        </Box>
                    </Box>
                    {/*<Box sx={styles.imageUploader} onClick={handleUploadClick}>*/}
                    {/*    <Image src={downArrow} />*/}
                    {/*    <Typography>Загрузить фото</Typography>*/}
                    {/*    <span>png / jpeg</span>*/}
                    {/*</Box>*/}
                    {/*{imageSrc && (imageSrc.toLowerCase().includes('http') || imageSrc.length > 100) && <img src={imageSrc.indexOf('data:image') === -1 ? `data:image/png;base64,${imageSrc}` : imageSrc}*/}
                    {/*    alt="Uploaded"*/}
                    {/*    style={styles.avatar} />}*/}
                    <input
                        type="file"
                        accept="image/*"
                        ref={fileInputRef}
                        style={{ display: 'none' }}
                        onChange={handleFileChange}
                    />
                </Box>
            </Box>
        </Container>
    );
}
