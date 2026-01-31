import styled from '@emotion/styled';
import { Button, message, Radio, RadioChangeEvent, Spin } from 'antd';
import { useEffect, useState } from 'react';

import { useGetAdminGoodsByFilter } from '~/api/v1/useGetAdminGoodsByFilter';
import { usePutAdminGoods } from '~/api/v1/usePutAdminGoods';
import { Pagination } from '~/components';
import { PageLayout } from '~/components/PageLayout';
import { usePageNumber } from '~/hooks';
import { MerchandiseReadModel } from '~/types';
import { ProductsList } from '~/views/ProductsView/ProductsList';

import { AdminProductsCommentModal } from './AdminProductsCommentModal/AdminProductsCommentModal';
import AdminProductsEditModal from './AdminProductsEditModal/AdminProductsEditModal';

enum SORT_TYPE {
  ALL = 'ALL',
  APPROVED = 'APPROVED',
  NOT_APPROVED = 'NOT_APPROVED',
  NEW = 'NEW',
}

type SORT_TYPE_RECORD = { [K in SORT_TYPE]: boolean | null };

function isSortType(value: string): value is SORT_TYPE {
  return Object.values<string>(SORT_TYPE).includes(value);
}

const StyledButton = styled(Button)`
  margin-bottom: 10px;
`;

type ADMIN_PRODUCT_FILTERS = {
  isTagsAppovedByAdmin: boolean | null;
  isNew: boolean | null;
};

const AdminProducts = () => {
  const pageSize = 10;
  const [filters, setFilters] = useState<ADMIN_PRODUCT_FILTERS>({
    isTagsAppovedByAdmin: null,
    isNew: null,
  });
  const [pageNumber] = usePageNumber();
  const goods = useGetAdminGoodsByFilter({ pageNumber, pageSize, ...filters });
  const pagesCount = goods.data && Math.ceil(goods.data.totalCount / pageSize);
  const mutation = usePutAdminGoods();
  const [editId, updateEditId] = useState<boolean | string>(false);
  const [addCommentProduct, updateAddCommentProduct] = useState<MerchandiseReadModel | undefined>(undefined);

  const onChangeSort = (e: RadioChangeEvent) => {
    const { value } = e.target;

    const mappingTags: SORT_TYPE_RECORD = {
      [SORT_TYPE.ALL]: null,
      [SORT_TYPE.APPROVED]: true,
      [SORT_TYPE.NOT_APPROVED]: false,
      [SORT_TYPE.NEW]: null,
    };

    const mappingNew: SORT_TYPE_RECORD = {
      [SORT_TYPE.ALL]: null,
      [SORT_TYPE.APPROVED]: null,
      [SORT_TYPE.NOT_APPROVED]: null,
      [SORT_TYPE.NEW]: true,
    };

    if (isSortType(value)) {
      setFilters({
        isNew: mappingNew[value],
        isTagsAppovedByAdmin: mappingTags[value],
      });
    }
  };

  const onApprove = (record: MerchandiseReadModel) => {
    mutation.mutate({
      ...record,
      categories:
        record.categories?.map((item) => {
          return item.id;
        }) ?? [],
      isTagsAppovedByAdmin: true,
    });
  };

  const handleEditModalClose = () => {
    updateEditId(false);
  };

  const handleEditModalSuccess = () => {
    updateEditId(false);
    goods.refetch();
  };

  const handleAddCommentModalClose = () => {
    updateAddCommentProduct(undefined);
  };

  const handleAddCommentModalSuccess = () => {
    goods.refetch();

    updateAddCommentProduct(undefined);
  };

  useEffect(() => {
    if (mutation.isSuccess) {
      goods.refetch();

      message.success('Товар подтвержден');
    }
  }, [mutation.isSuccess]);

  const renderCustomActions = (merch: MerchandiseReadModel) => {
    return (
      <>
        <StyledButton
          onClick={(e) => {
            updateEditId(merch.id);

            e.stopPropagation();
            e.preventDefault();
          }}
        >
          Редактировать теги
        </StyledButton>
        {!merch.isTagsAppovedByAdmin ? (
          <StyledButton
            onClick={(e) => {
              onApprove(merch);

              e.stopPropagation();
              e.preventDefault();
            }}
          >
            Подтвердить
          </StyledButton>
        ) : undefined}
        <StyledButton
          style={{
            fontSize: '80%',
          }}
          type="primary"
          onClick={(e) => {
            updateAddCommentProduct(merch);

            e.stopPropagation();
            e.preventDefault();
          }}
        >
          Сообщить о нарушении
        </StyledButton>
      </>
    );
  };

  return (
    <PageLayout title="Управление продуктами">
      <Radio.Group defaultValue="ALL" onChange={onChangeSort}>
        <Radio.Button value="ALL">Все товары</Radio.Button>
        <Radio.Button value="APPROVED">Подтвержденные</Radio.Button>
        <Radio.Button value="NOT_APPROVED">Требуют подтверждения</Radio.Button>
        <Radio.Button value="NEW">Новые товары</Radio.Button>
      </Radio.Group>

      {goods.data?.items ? (
        <ProductsList items={goods.data?.items} addToCard={false} customActions={renderCustomActions} showTags />
      ) : (
        <Spin />
      )}

      <AdminProductsEditModal editId={editId} onClose={handleEditModalClose} onSuccess={handleEditModalSuccess} />
      <AdminProductsCommentModal
        product={addCommentProduct}
        onSuccess={handleAddCommentModalSuccess}
        onClose={handleAddCommentModalClose}
      />
      <Pagination pageNumber={pageNumber} pagesCount={pagesCount} />
    </PageLayout>
  );
};

export default AdminProducts;
