import { Theme, useMediaQuery } from '@mui/material';

type UseDownMd = () => boolean;

const useDownMd: UseDownMd = () => useMediaQuery<Theme>((theme) => theme.breakpoints.down('md'));

export { useDownMd };
