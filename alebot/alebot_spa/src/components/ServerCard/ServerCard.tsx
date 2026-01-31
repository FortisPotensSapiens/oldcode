import React, {useState} from 'react';
import { Box, Typography } from "@mui/material";
import { styles } from '../ProductCard/style';
import Image from "next/image";
import Button from "@mui/material/Button";
import { isImageFileExtensionValid } from "@/helpers/imageValidator";
import productImage from '../../assets/images/productimage.jpeg'
import Link from "next/link";
import MuiMarkdown from 'mui-markdown';
import {color} from "@mui/system";
import Pen from "@/assets/svg/Pen";
import api from "@/service/api";
type Props = {
    photo: string,
    name: string,
    price: number,
    shortDescription: string,
    id: string,
    licenseKey?:string,
    tradingAccountNumber?:string,
    login?:string,
    password?:string,
    color?:string,
    created?:string,
    expirationDate?:string,
    serverAddress?:string,
}
const ServerCard: React.FC<Props> = ({ photo,
                                          name,
                                          price,
                                          shortDescription,
                                          id,
                                          handleModalOpen,
                                          licenseKey,
                                          tradingAccountNumber,
                                          login,
                                          password,
                                          color,
                                          created,
                                          expirationDate,
                                          serverAddress,
                                          userServerId
                                      }) => {
    const [popover,setPopover] = useState('none')
    const handleUpdate = ()=>{
        api.put(`/api/v1/vds-server/reboot/${userServerId}`)
            .then(res => {
                if (res.status === 200) {
                    setPopover('success')
                }else {
                    setPopover('err')
                }
            })
            .catch(err => setPopover('err'))
    }
    return (
        <Box sx={{
            width: '100%',
            borderRadius: '20px',
            background: '#FFF',
            padding: '0 32px',
            maxWidth: '100%',
            marginTop: '20px',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'space-between',
            paddingBottom:'20px',
            border:'0.5px solid #D9D9D9',
            boxShadow:'0 2px 4px rgb(0 0 0 / 27%)',

        }}>
            <Box sx={{display:'flex',flexDirection:'column',justifyContent:'flex-start'}}>
                <Link href={`/products/[id]`} as={`/products/${id}`} style={{
                    color: '#000',
                    fontSize: '10px',
                    fontWeight: '400',
                    lineHeight: 'normal',
                    marginTop: '14px',
                    textAlign:'center',
                }}>
                    <Image src={isImageFileExtensionValid(photo) ? `data:image/png;base64,${photo}` : productImage}
                           alt={name}
                           style={styles.photo}
                           width={200}
                           height={200}
                    />
                </Link>
                <Typography sx={{...styles.name,marginTop:'16px'}} className={color === 'green' ? 'color-green' : ''}>{name}</Typography>
                {licenseKey && <Typography sx={{...styles.licenseKey, marginTop: '16px'}}>Ключ лицензи: {licenseKey}</Typography>}
                {tradingAccountNumber && <Typography sx={{...styles.licenseKey}} onClick={()=> handleModalOpen(id)}>Номер счета: <span>{tradingAccountNumber}  <Pen/> &nbsp;&nbsp;&nbsp;</span></Typography>}
                {login && <Typography sx={styles.mobileTitles}>
                            <b>Логин</b>
                            <span>{login}</span>
                          </Typography>}
                {password && <Typography sx={styles.mobileTitles}>
                               <b>Пароль</b>
                               <span>{password}</span>
                             </Typography>}
                {serverAddress && <Typography sx={styles.mobileTitles}>
                    <b>Адресс Сервера</b>
                    <span>{serverAddress}</span>
                </Typography>}
                {created && <Typography sx={styles.mobileTitles}>
                               <b>Дата создания</b>
                               <span>{new Date(created).toISOString().slice(0, 19).replace("T", " ")}</span>
                             </Typography>}
                {expirationDate && <Typography sx={styles.mobileTitles}>
                    <b>Оплачен до</b>
                    <span>{new Date(expirationDate).toISOString().slice(0, 19).replace("T", " ")}</span>
                </Typography>}
                <br />
                <Box sx={{textAlign:'left',}}>
                    <MuiMarkdown>
                        {shortDescription}
                    </MuiMarkdown>
                </Box>
            </Box>
            <Box sx={{display:'flex',flexDirection:'column',justifyContent:'center',alignItems:'center'}}>
                {login && <Button sx={styles.button} onClick={handleUpdate} style={{backgroundColor:'#404040'}}>Перезагрузить</Button>}
                <Button sx={{...styles.button,marginTop:'10px'}} onClick={() => handleModalOpen(id, userServerId)} className={color === 'green' ? 'bg-green' : ''}>{login ? 'Продлить' : 'Купить'}</Button>
                {price && <Typography sx={styles.price} className={color === 'green' ? 'color-green' : ''}>{price} USDT</Typography>}
                {popover === 'err' || popover === 'success' ? <Typography className={popover === 'err' ? 'color-red' : 'color-green'} sx={{
                    fontSize: '12px',
                    fontWeight:'bold',
                    marginTop: '10px',
                    textAlign: 'center',
                }}>{popover === "err" ? 'Произошла ошибка. Попробуйте повторить позже' : 'Сервер обновлен'}</Typography> : ''}
            </Box>
        </Box>

    );
};

export default ServerCard;