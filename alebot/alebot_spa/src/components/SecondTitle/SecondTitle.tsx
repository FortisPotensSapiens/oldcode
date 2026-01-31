import React, {CSSProperties, ReactNode} from 'react';
import {Typography} from "@mui/material";
type Props = {
    style?: CSSProperties;
    children: ReactNode;
}
const SecondTitle:React.FC<Props> = ({children}) => {
    return (
        <Typography sx={{
            color: '#1E1E1E',
            fontSize: '20px',
            fontWeight:'600',
            lineHeight:'normal',
        }}>{children}</Typography>
    );
};

export default SecondTitle;