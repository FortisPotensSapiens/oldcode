import styled from '@emotion/styled';
import type { TabsProps } from 'antd';
import { Tabs } from 'antd';
import { useMemo } from 'react';

import { useGetPartnerById } from '~/api';
import { useGetSellerGoodsByFilter } from '~/api/v1/useGetSellerGoodsByFilter';
import { useConfig } from '~/contexts';
import { usePageNumber } from '~/hooks';

import { ProductsView } from '../ProductsView';
import FeedbackTab from './FeedbackTab/FeedbackTab';

const TabsContainer = styled.div`
  background: #fafafa;
  padding: 5px 20px;
  margin-top: 20px;
  margin-bottom: 30px;
  border-radius: 10px;
  box-shadow: -2px -2px 2px rgb(0 0 0 / 2%), 2px 2px 2px rgb(0 0 0 / 2%);
`;

const SellerFeedbackTabs = ({ sellerId }: { sellerId: string }) => {
  const seller = useGetPartnerById(sellerId);
  const [pageNumber] = usePageNumber();
  const { storefront } = useConfig();
  const { pageSize } = storefront;
  const goods = useGetSellerGoodsByFilter({ pageNumber, pageSize, partnerId: sellerId });

  const tabs: TabsProps['items'] = useMemo(
    () => [
      {
        key: '1',
        label: 'Товары продавца',
        children: <ProductsView hideSeller {...goods} isGlobalLoading={seller.isLoading} />,
      },
      {
        key: '2',
        label: 'Отзывы',
        children: <FeedbackTab productId={sellerId} />,
      },
    ],
    [goods, seller.isLoading, sellerId],
  );

  return (
    <TabsContainer>
      <Tabs defaultActiveKey="1" items={tabs} />
    </TabsContainer>
  );
};

export default SellerFeedbackTabs;
