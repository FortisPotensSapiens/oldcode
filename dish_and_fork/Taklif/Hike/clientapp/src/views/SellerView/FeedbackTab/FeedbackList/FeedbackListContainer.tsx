import { List, Rate, Spin, Typography } from 'antd';
import dayjs from 'dayjs';
import LocalizedFormat from 'dayjs/plugin/localizedFormat';
import { useCallback, useEffect, useState } from 'react';
import InfiniteScroll from 'react-infinite-scroll-component';

import { useGetPartnerRatingByFilter } from '~/api/v1/useGetPartnerRatingByFilter';
import { PartnerRatingReadModel } from '~/types';

const { Text } = Typography;

dayjs.extend(LocalizedFormat);

const PAGE_SIZE = 10;

const FeedbackListContainer = ({ productId }: { productId: string }) => {
  const [currentPage, setCurrentPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const { data } = useGetPartnerRatingByFilter({
    pageNumber: currentPage,
    pageSize: PAGE_SIZE,
    partnerId: productId,
  });

  const [allItems, setAllItems] = useState<PartnerRatingReadModel[]>([]);

  useEffect(() => {
    if (data) {
      const newValue = [...allItems, ...(data?.items ?? [])];

      newValue.sort((a, b) => (dayjs(a.created).isAfter(b.created) ? 1 : -1));

      setAllItems(newValue);
      setHasMore(data.totalCount > currentPage * PAGE_SIZE);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [data]);

  useEffect(() => {
    setAllItems([]);
  }, []);

  const handleLoad = useCallback(() => {
    setCurrentPage(currentPage + 1);
  }, [setCurrentPage, currentPage]);

  return (
    <InfiniteScroll dataLength={allItems.length} next={handleLoad} hasMore={hasMore} loader={<Spin />}>
      <List
        locale={{ emptyText: 'У этого магазина пока нет отзывов.' }}
        dataSource={allItems}
        renderItem={(item) => (
          <List.Item key={item.id}>
            <List.Item.Meta
              title={dayjs(item.created).format('LLL')}
              description={
                <>
                  <Rate value={item.rating} disabled style={{ color: '#7e004a' }} />

                  <Typography>
                    <Text>{item.comment}</Text>
                  </Typography>
                </>
              }
            />
          </List.Item>
        )}
      />
    </InfiniteScroll>
  );
};

export default FeedbackListContainer;
