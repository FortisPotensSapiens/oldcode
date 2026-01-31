import styled from '@emotion/styled';
import AddIcon from '@mui/icons-material/Add';
import { Box, Button } from '@mui/material';
import { Radio, RadioChangeEvent, Spin } from 'antd';
import { useSnackbar } from 'notistack';
import { FC, useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

import { useGetMyPartnership } from '~/api';
import { useDeleteGood } from '~/api/v1/useDeleteGood';
import { useGetPartnerGoodsByFilter } from '~/api/v1/useGetPartnerGoodsByFilter';
import { usePutPublishGood } from '~/api/v1/usePutPublishGood';
import { usePutUnpublishGood } from '~/api/v1/usePutUnpublishGood';
import { EmptyList, Error, PageLayout, TABLE_PAGINATION_ROW_COUNT_ITEMS, TablePagination } from '~/components';
import ConfirmDeleteDialog from '~/components/ConfirmDeleteDialog/ConfirmDeleteDialog';
import FabButton from '~/components/FabButton/FabButton';
import { useDownSm } from '~/hooks';
import { FilterMerchandiseByPartnerForPartnerDetailsModel, MerchandiseReadModel, MerchandisesState } from '~/types';
import { ProductsList } from '~/views/ProductsView/ProductsList';

import { useNavigateNewProduct } from './useNavigateNewProduct';

const StyledButton = styled(Button)`
  margin-bottom: 10px;
`;

const NoData = styled.div({
  padding: '2rem 0 5rem 0',
  textAlign: 'center',
});

enum PARTNER_FILTERS {
  ALL = 'ALL',
  ACTIVE = 'ACTIVE',
  OUT_OF_STOCK = 'OUT_OF_STOCK',
  BLOCKED_BY_ADMIN = 'BLOCKED_BY_ADMIN',
  ON_MODERATION = 'ON_MODERATION',
  HIDEN_BY_ME = 'HIDEN_BY_ME',
}

const PARTNER_FILTERS_TRANSLATE: Record<string, string> = {
  [PARTNER_FILTERS.ACTIVE]: 'Активные',
  [PARTNER_FILTERS.OUT_OF_STOCK]: 'Закончились',
  [PARTNER_FILTERS.ON_MODERATION]: 'Ожидают модерации',
  [PARTNER_FILTERS.BLOCKED_BY_ADMIN]: 'Требуют доработки',
  [PARTNER_FILTERS.ALL]: 'Все',
  [PARTNER_FILTERS.HIDEN_BY_ME]: 'Скрытые мной',
};

const PartnerProducts: FC = () => {
  const { enqueueSnackbar } = useSnackbar();
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(TABLE_PAGINATION_ROW_COUNT_ITEMS[0]);
  const { data: partnership } = useGetMyPartnership();
  const createProduct = useNavigateNewProduct();
  const [deleteGoodId, setDeleteGoodId] = useState<undefined | string>(undefined);
  const deleteGood = useDeleteGood();
  const publish = usePutPublishGood();
  const unpublish = usePutUnpublishGood();
  const navigate = useNavigate();
  const [currentFilter, updateCurrentFilter] = useState<PARTNER_FILTERS>(PARTNER_FILTERS.ALL);
  const [filters, updateFilters] = useState<Partial<FilterMerchandiseByPartnerForPartnerDetailsModel>>({
    showHidden: undefined,
    showOutOfStock: undefined,
    showOnModeration: undefined,
    showBlockedByAdmin: undefined,
  });

  const { data, isError, isSuccess, refetch } = useGetPartnerGoodsByFilter(
    {
      partnerId: partnership?.id ?? '',
      pageNumber: page + 1,
      pageSize: rowsPerPage,
      ...filters,
    },
    {
      enabled: !!partnership?.id,
    },
  );

  const handleChangePage = (_event: unknown, newPage: number) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  const getPagination = () => {
    return (
      <TablePagination
        colSpan={6}
        count={data?.totalCount}
        onPageChange={handleChangePage}
        onRowsPerPageChange={handleChangeRowsPerPage}
        page={page}
        rowsPerPage={rowsPerPage}
      />
    );
  };

  const onTogglePublishCallback = async (goodId: string, state: boolean) => {
    try {
      if (state) {
        await publish.mutateAsync({
          goodId,
        });
      } else {
        await unpublish.mutateAsync({
          goodId,
        });
      }

      enqueueSnackbar('Статус товара обновлен', {
        autoHideDuration: 2000,
        variant: 'info',
      });

      refetch();
    } catch (e) {
      //*
    }
  };

  const onDelete = (offerId: string) => {
    setDeleteGoodId(offerId);
  };

  const onDeleteClose = () => {
    setDeleteGoodId(undefined);
  };

  const onDeleteConfirmed = () => {
    if (deleteGoodId) {
      deleteGood.mutate({
        goodId: deleteGoodId,
      });
    }
  };

  const handleFilterChange = (e: RadioChangeEvent) => {
    const { value } = e.target;

    const mapping: Record<string, Partial<FilterMerchandiseByPartnerForPartnerDetailsModel>> = {
      [PARTNER_FILTERS.ALL]: {
        showBlockedByAdmin: undefined,
        showOnModeration: undefined,
        showOutOfStock: undefined,
        showHidden: undefined,
      },
      [PARTNER_FILTERS.ACTIVE]: {
        showHidden: false,
        showOutOfStock: false,
        showOnModeration: false,
        showBlockedByAdmin: false,
      },
      [PARTNER_FILTERS.OUT_OF_STOCK]: {
        showHidden: false,
        showOutOfStock: true,
        showOnModeration: false,
        showBlockedByAdmin: false,
      },
      [PARTNER_FILTERS.ON_MODERATION]: {
        showHidden: false,
        showOutOfStock: false,
        showOnModeration: true,
        showBlockedByAdmin: false,
      },
      [PARTNER_FILTERS.BLOCKED_BY_ADMIN]: {
        showHidden: false,
        showOutOfStock: false,
        showOnModeration: false,
        showBlockedByAdmin: true,
      },
      [PARTNER_FILTERS.HIDEN_BY_ME]: {
        showHidden: true,
        showOutOfStock: false,
        showOnModeration: false,
        showBlockedByAdmin: false,
      },
    };

    updateFilters({
      ...filters,
      ...(mapping[value] ?? {}),
    });
    updateCurrentFilter(value);
  };

  useEffect(() => {
    if (deleteGood.isSuccess) {
      enqueueSnackbar('Ваш товар удален', {
        variant: 'success',
      });
      refetch();
      onDeleteClose();
    }
  }, [deleteGood.isSuccess, enqueueSnackbar, refetch]);

  const renderCustomActions = (merch: MerchandiseReadModel) => {
    return (
      <>
        <StyledButton
          onClick={(e) => {
            navigate(`/partner/products/${merch.id}`);

            e.stopPropagation();
            e.preventDefault();
          }}
        >
          Редактировать
        </StyledButton>
        <StyledButton
          onClick={(e) => {
            e.stopPropagation();
            e.preventDefault();

            onTogglePublishCallback(merch.id, merch.state === MerchandisesState.Created);
          }}
        >
          {merch.state === MerchandisesState.Created ? 'Опубликовать' : 'Скрыть'}
        </StyledButton>
        <StyledButton
          variant="contained"
          onClick={(e) => {
            onDelete(merch.id);

            e.stopPropagation();
            e.preventDefault();
          }}
        >
          Удалить
        </StyledButton>
      </>
    );
  };

  if (isError) {
    return <Error />;
  }

  return (
    <PageLayout
      title={
        <div>
          Мои товары
          <Box
            borderRadius={50}
            component={Button}
            display={{ sm: 'inline-flex', xs: 'none' }}
            height={50}
            ml={3}
            onClick={createProduct}
            size="large"
            startIcon={<AddIcon />}
            variant="contained"
          >
            Добавить товар
          </Box>
        </div>
      }
    >
      <>
        <FabButton onClick={createProduct} />

        <Radio.Group value={currentFilter} onChange={handleFilterChange}>
          {Object.keys(PARTNER_FILTERS).map((filter) => {
            return (
              <Radio.Button value={filter} key={filter}>
                {PARTNER_FILTERS_TRANSLATE[filter]}
              </Radio.Button>
            );
          })}
        </Radio.Group>

        {!isSuccess ? (
          <NoData>
            <Spin />
          </NoData>
        ) : (
          <>
            {data?.items?.length ? (
              <ProductsList customActions={renderCustomActions} items={data.items} addToCard={false} showTags />
            ) : (
              <NoData>Нет товаров.</NoData>
            )}
          </>
        )}

        {getPagination()}
      </>

      <ConfirmDeleteDialog
        text="Вы уверены что хотите удалить товар?"
        itemId={deleteGoodId}
        onClose={onDeleteClose}
        onDelete={onDeleteConfirmed}
      />
    </PageLayout>
  );
};

export { PartnerProducts };
