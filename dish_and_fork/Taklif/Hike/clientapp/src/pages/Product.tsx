import { useOidc, useOidcUser } from '@axa-fr/react-oidc';
import { notification } from 'antd';
import { FC, useEffect } from 'react';

import { useGetGoodById, useGetMyUserProfile } from '~/api';
import { usePutRequestCompositions } from '~/api/v1/usePutRequestCompositions';
import { useProductParams } from '~/routing';
import { ProductView, ProductViewProps } from '~/views';

type ProductProps = Pick<ProductViewProps, 'hideSeller'>;

const Product: FC<ProductProps> = ({ hideSeller }) => {
  const { productId = '' } = useProductParams();
  const product = useGetGoodById(productId);
  const requestCompositionMutation = usePutRequestCompositions();
  const [api, contextHolder] = notification.useNotification();
  const { data: user } = useGetMyUserProfile();

  const handleRequestComposition = (goodId: string) => {
    requestCompositionMutation.mutateAsync({
      goodId,
    });
  };

  useEffect(() => {
    if (requestCompositionMutation.isSuccess) {
      api.success({
        message: 'Ваш запрос успешно отправлен в магазин.',
      });

      product.refetch();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [api, requestCompositionMutation.isSuccess]);

  return (
    <>
      {contextHolder}
      <ProductView
        userId={user?.id}
        isCompositionRequestLoading={requestCompositionMutation.isLoading}
        hideSeller={hideSeller}
        onRequestComposition={handleRequestComposition}
        {...product}
      />
    </>
  );
};

export default Product;
