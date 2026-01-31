import { TypographyProps } from '@mui/material';

import { useDownMd, useDownSm } from '~/hooks';

type UseVariant = () => TypographyProps['variant'];

const useVariant: UseVariant = () => {
  const isSmall = useDownSm();
  const isMiddle = useDownMd();

  if (isSmall) {
    return 'h5';
  }

  if (isMiddle) {
    return 'h4';
  }

  return 'h3';
};

export default useVariant;
