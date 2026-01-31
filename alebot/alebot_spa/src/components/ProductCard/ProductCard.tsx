import React, { useState } from 'react';
import { Box, Typography } from "@mui/material";
import { styles } from './style';
import Image from "next/image";
import Button from "@mui/material/Button";
import { isImageFileExtensionValid } from "@/helpers/imageValidator";
import productImage from '../../assets/images/productimage.jpeg'
import Link from "next/link";
import MuiMarkdown from 'mui-markdown';
import { color } from "@mui/system";
import Pen from "@/assets/svg/Pen";
import LicenseModal from "@/components/LicenseModal/LicenseModal";
import { useRouter } from 'next/router';

type Props = {
    photo: string,
    name: string,
    price: number,
    shortDescription: string,
    id: string,
    licenseKey?: string,
    tradingAccountNumber?: string,
    color?: string,
}
const ProductCard: React.FC<Props> = ({ photo,
    name,
    price,
    shortDescription,
    id,
    handleModalOpen,
    licenseKey,
    tradingAccountNumber,
    color,
    isBuyed,
    userLicenseId
}) => {
    const router = useRouter();
    const [showModal, setShowModal] = useState(false)
    const setAccountNumberChanged = () => {
        window.location.reload(false);
    }
    return (
        <Box sx={{
            width: '100%',
            borderRadius: '20px',
            background: '#FFF',
            padding: '0 32px',
            maxWidth: '1440px',
            marginTop: '20px',
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'space-between',
            paddingBottom: '20px',
            border: '0.5px solid #D9D9D9',
            boxShadow: '0 2px 4px rgb(0 0 0 / 27%)',

        }}>
            <Box sx={{display:'flex',flexDirection:'column',justifyContent:'flex-start'}}>
                <LicenseModal onPress={setShowModal} active={showModal} id={userLicenseId} onNumberChanged={setAccountNumberChanged} />
                <Link href={`/products/[id]`} as={`/products/${id}`} style={{
                    color: '#000',
                    fontSize: '10px',
                    fontWeight: '400',
                    lineHeight: 'normal',
                    marginTop: '14px',
                    textAlign: 'center',
                }} >
                    <Image src={isImageFileExtensionValid(photo) ? `data:image/png;base64,${photo}` : productImage}
                           alt={name}
                           style={styles.photo}
                           width={200}
                           height={200}
                    />
                </Link>
                <Typography sx={{ ...styles.name, marginTop: '16px' }} className={color === 'green' ? 'color-green' : ''}>{name}</Typography>
                {licenseKey && <Typography sx={{ ...styles.licenseKey, marginTop: '16px',fontSize:'20px' }}>Ключ лицензи: {licenseKey}</Typography>}
                {isBuyed && <Typography sx={{ ...styles.licenseKey,fontSize:'20px' }}>Номер счета: <span onClick={() => {
                    setShowModal(true)
                }}>{tradingAccountNumber}  <Pen /> &nbsp;&nbsp;&nbsp;</span></Typography>}
                <br />
                {!isBuyed && <Box sx={{
                    textAlign: 'left',
                }}>
                    <MuiMarkdown>
                        {shortDescription}
                    </MuiMarkdown>
                </Box>}
            </Box>
            {!isBuyed && <Box>
                {!isBuyed && <Button sx={styles.button} onClick={() => handleModalOpen(id)}
                                  className={color === 'green' ? 'bg-green' : ''}>Купить</Button>}
                {!isBuyed && <Typography sx={styles.price}
                                      className={color === 'green' ? 'color-green' : ''}>{price} USDT</Typography>}
            </Box>}
        </Box>

    );
};

export default ProductCard;