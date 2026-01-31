import React from 'react';
import { Box, Typography } from "@mui/material";
import { styles } from './style'
import user from '../../assets/images/user.png';
import Image from "next/image";
type Props = {
    active: number;
    email: string;
    fullName: string;
    productCount: number;
    total: number;
    myIncome: number,
}
function getProductText(count: number): string {
    const i = count % 10;
    if (i == 0 || (count > 10 && count < 20))
        return 'продуктов';
    if (i > 4)
        return 'продуктов';
    if (i > 1)
        return 'продукта';
    return 'продукт';
}

const ReferralCard: React.FC<Props> = ({ active, email, fullName, productCount, total, myIncome }) => {
    return (
        <Box sx={styles.box}>
            <Image src={user} />
            <Typography sx={styles.title}>{fullName}</Typography>
            <Typography sx={styles.email}>{email}</Typography>
            <Typography sx={styles.myEarn}>Мой доход</Typography>
            <Typography sx={styles.price}>$ {myIncome}</Typography>
            <Typography sx={styles.active}>{active}/{total} активных</Typography>
            <Typography sx={styles.product}>{productCount} {getProductText(productCount)}</Typography>
        </Box>
    );
};

export default ReferralCard;