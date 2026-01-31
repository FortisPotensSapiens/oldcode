import styled from '@emotion/styled';
import { Box, BoxProps, ButtonProps, Typography } from '@mui/material';
import { Button, Tooltip } from 'antd';
import { FC, useMemo } from 'react';

import config from '~/config';
import { CartItem } from '~/contexts';
import { useCartSum, useCurrencySymbol } from '~/hooks';

type CartSummaryProps = BoxProps &
  Pick<ButtonProps, 'disabled'> & {
    cart: CartItem[];
  };

const StyledButton = styled(Button)({
  width: '100%',
});

const CartSummary: FC<CartSummaryProps> = ({ cart, disabled, ...props }) => {
  const currency = useCurrencySymbol();
  const sum = useCartSum(cart);

  const handleClick = (e: any) => {
    props.onClick?.(e);
  };

  const SubmitButton = useMemo(() => {
    return (
      <StyledButton onClick={handleClick} disabled={config.isTestingMode ? true : disabled} type="primary">
        Оформить заказ
      </StyledButton>
    );
  }, [handleClick]);

  return (
    <Box mt={2}>
      <Box alignItems="flex-end" display="flex" flexGrow={1} marginBottom={3}>
        <Typography flexGrow={1}>Итого:</Typography>

        <Typography>
          {sum.toLocaleString()} {currency}
        </Typography>
      </Box>

      {config.isTestingMode ? (
        <Tooltip title="Сайт находится в тестовом режиме. Заказ товаров будет доступен немного позднее :-)">
          <div>{SubmitButton}</div>
        </Tooltip>
      ) : (
        SubmitButton
      )}
    </Box>
  );
};

export { CartSummary };
