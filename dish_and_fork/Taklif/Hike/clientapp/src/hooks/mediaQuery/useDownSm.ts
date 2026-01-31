import { Theme, useMediaQuery } from '@mui/material';

type UseDownSm = () => boolean;

const useDownSm: UseDownSm = () => useMediaQuery<Theme>((theme) => theme.breakpoints.down('sm'));

export { useDownSm };
