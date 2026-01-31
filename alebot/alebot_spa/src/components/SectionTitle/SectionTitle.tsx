import React, {CSSProperties, ReactNode} from 'react';
import {Typography} from "@mui/material";
type Props = {
    style?: CSSProperties;
    children: ReactNode;
}
const SectionTitle:React.FC<Props> = ({children,style}) => {
    return (
        <Typography style={style as  React.CSSProperties}
            sx={{
            color: '#1E1E1E',
            fontSize: '32px',
            fontWeight: 700,
            lineHeight:'normal',
        }}>{children}</Typography>
    );
};

export default SectionTitle;