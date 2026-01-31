import { ExclamationCircleFilled } from '@ant-design/icons';
import { Box, IconButton, Paper } from '@mui/material';
import { Modal } from 'antd';
import { FC } from 'react';

import { useGetPartnerById } from '~/api';
import { ReactComponent as TrashIcon } from '~/assets/icons/trash.svg';
import { CartList, CartListProps, CartSummary, EllipsisBox } from '~/components';
import { PartnerReadModel } from '~/types';

import useClearCart from './useClearCart';
import useClick from './useClick';
import useSortItems from './useSortItems';

const { confirm } = Modal;

type SellerCartProps = Pick<CartListProps, 'items'> & PartnerReadModel;

const SellerCart: FC<SellerCartProps> = ({ items, ...partner }) => {
  const { data = partner, isLoading, isError } = useGetPartnerById(partner.id);
  const sorted = useSortItems(items);
  const placeOrder = useClick(partner.id);
  const clearCart = useClearCart(items);
  const disabled = isLoading || isError;

  const showConfirm = () => {
    confirm({
      title: 'Вы уверены что хотите удалить эту корзину?',
      icon: <ExclamationCircleFilled />,
      cancelText: 'Отменить',
      okText: 'Удалить',
      onOk() {
        clearCart();
      },
    });
  };

  return (
    <Box
      borderRadius={{ sm: 2.5, xs: 0 }}
      component={Paper}
      display="flex"
      elevation={1}
      flexDirection="column"
      minHeight={{ sm: 1 }}
      overflow="hidden"
      position="relative"
      px={2}
      py={3}
    >
      <Box component={IconButton} mr={1} mt={1} onClick={showConfirm} position="absolute" right={0} top={0} zIndex={1}>
        <TrashIcon />
      </Box>

      <Box alignItems="center" display="flex" flexDirection="column" mb={2} overflow="hidden" position="relative">
        <Box
          bgcolor="grey.400"
          borderRadius="50%"
          height={64}
          style={data.image?.path ? { backgroundImage: `url(${data.image.path})` } : {}}
          sx={{ backgroundPosition: 'center center', backgroundSize: 'cover' }}
          width={64}
        />

        <EllipsisBox mt={1.5} textAlign="center" typography="h5" width={1}>
          {data.title}
        </EllipsisBox>
      </Box>

      <Box flexGrow={1}>
        <CartList items={sorted} />
      </Box>

      <CartSummary cart={items} disabled={disabled} onClick={placeOrder} />
    </Box>
  );
};

export { SellerCart };
