import styled from '@emotion/styled';
import type { TabsProps } from 'antd';
import { Tabs } from 'antd';
import { useMemo } from 'react';

import FeedbackTab from './FeedbackTab/FeedbackTab';

const TabsContainer = styled.div`
  background: #fff;
  padding: 5px 20px;
  margin-bottom: 30px;
  border-radius: 10px;
  box-shadow: -2px -2px 2px rgb(0 0 0 / 2%), 2px 2px 2px rgb(0 0 0 / 2%);
`;

const ProductViewTabs = ({ productId }: { productId: string }) => {
  const tabs: TabsProps['items'] = useMemo(
    () => [
      {
        key: '1',
        label: 'Отзывы',
        children: <FeedbackTab productId={productId} />,
      },
    ],
    [productId],
  );

  return (
    <TabsContainer>
      <Tabs defaultActiveKey="1" items={tabs} />
    </TabsContainer>
  );
};

export default ProductViewTabs;
