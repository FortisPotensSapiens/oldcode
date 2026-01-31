import styled from '@emotion/styled';
import { Button, Collapse, List, message, Spin } from 'antd';
import { useEffect, useMemo, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { useDeleteCategoryById } from '~/api/v1/useDeleteCategoryById';
import { CONFIG_ENUM_KEYS, useGetConfigEnums } from '~/api/v1/useGetConfigEnums';
import { usePostAdminCategories } from '~/api/v1/usePostAdminCategories';
import { usePostCategoriesFilter } from '~/api/v1/usePostCategoriesByFilter';
import { usePutAdminCategories } from '~/api/v1/usePutAdminCategories';
import { PageLayout } from '~/components';
import { CategoryCreateModel, CategoryReadModel, CategoryUpdateModel } from '~/types';

import AdminTagsAddModal from './AdminTagsAddModal/AdminTagsAddModal';
import AdminTagsItem from './AdminTagsItem/AdminTagsItem';

const CollapseContainer = styled.div`
  margin-top: 20px;
`;

const { Panel } = Collapse;

const AdminTags = () => {
  const { t } = useTranslation();
  const [addModalOpen, setAddModalOpen] = useState(false);
  const [updateModalCategory, setUpdateModalCategory] = useState<CategoryReadModel | null | undefined>(null);
  const deleteMutation = useDeleteCategoryById();
  const addMutation = usePostAdminCategories();
  const updateMutation = usePutAdminCategories();
  const { refetch, isLoading, data } = usePostCategoriesFilter({ pageNumber: 1, pageSize: 500, hideEmpty: false });

  const { data: configs } = useGetConfigEnums();
  const [sortedData, setSortedData] = useState<Record<string, CategoryReadModel[]>>({});

  const kinds = useMemo(() => {
    return configs?.[CONFIG_ENUM_KEYS.CategoryType] ?? [];
  }, [configs]);

  const onDeleteCallback = (item: CategoryReadModel) => {
    deleteMutation.mutate({
      categoryId: item.id,
    });
  };

  const handleShowToggle = (item: CategoryUpdateModel) => {
    updateMutation.mutate({
      ...item,
      showOnMainPage: !item.showOnMainPage,
    });
  };

  useEffect(() => {
    if (data?.items) {
      setSortedData(
        data.items.reduce<Record<string, CategoryReadModel[]>>((prev, item) => {
          if (item) {
            if (!prev[item.type]) {
              prev[item.type] = [];
            }

            prev[item.type].push(item);
          }

          return prev;
        }, {}),
      );
    }
  }, [data]);

  useEffect(() => {
    if (deleteMutation.isSuccess) {
      message.success('Тег успешно удален');
      refetch();
    }
  }, [refetch, deleteMutation.isSuccess]);

  useEffect(() => {
    if (deleteMutation.isError) {
      message.warning('Ошибка удаления тега');
    }
  }, [deleteMutation.isError]);

  useEffect(() => {
    if (updateMutation.isSuccess) {
      message.success('Тег успешно отредактирован');
      refetch();
      setUpdateModalCategory(null);
    }
  }, [refetch, updateMutation.isSuccess]);

  useEffect(() => {
    if (updateMutation.isError) {
      message.warning('Ошибка редактирования тега');
    }
  }, [updateMutation.isError]);

  const onAddModalOpen = () => {
    setAddModalOpen(true);
  };

  const addModalClose = () => {
    setAddModalOpen(false);
  };

  const onUpdateModalOpen = (data: CategoryReadModel) => {
    setUpdateModalCategory(data);
  };

  const updateModalClose = () => {
    setUpdateModalCategory(null);
  };

  const onAddSuccess = (data: CategoryCreateModel | CategoryUpdateModel) => {
    addMutation.mutate(data);
  };

  const onUpdateSuccess = (data: CategoryCreateModel | CategoryUpdateModel) => {
    updateMutation.mutate(data as CategoryUpdateModel);
  };

  useEffect(() => {
    if (addMutation.isSuccess) {
      message.success('Тег успешно добавлен');
      setAddModalOpen(false);
      refetch();
    }
  }, [refetch, addMutation.isSuccess]);

  useEffect(() => {
    if (addMutation.isError) {
      message.warning('Ошибка добавления тега');
    }
  }, [addMutation.isError]);

  return (
    <PageLayout title="Управление тегами">
      <Button type="primary" onClick={onAddModalOpen}>
        Добавить тег
      </Button>

      {isLoading ? <Spin /> : undefined}

      <CollapseContainer>
        <Collapse defaultActiveKey={['0']}>
          {kinds.map((item, key) => {
            return (
              // eslint-disable-next-line react/no-array-index-key
              <Panel header={t(`enums.type.${item.name}`)} key={key}>
                {item.name && sortedData[item.name] ? (
                  <List
                    itemLayout="horizontal"
                    dataSource={sortedData[item.name]}
                    renderItem={(item) => (
                      <AdminTagsItem
                        onShowToggle={handleShowToggle}
                        item={item}
                        onDelete={onDeleteCallback}
                        onUpdate={onUpdateModalOpen}
                      />
                    )}
                  />
                ) : undefined}
              </Panel>
            );
          })}
        </Collapse>
      </CollapseContainer>

      <AdminTagsAddModal
        isPending={addMutation.isLoading}
        title="Создание тега"
        open={addModalOpen}
        onClose={addModalClose}
        onSuccess={onAddSuccess}
      />

      {updateModalCategory ? (
        <AdminTagsAddModal
          isPending={updateMutation.isLoading}
          title="Редактирование тега"
          open={!!updateModalCategory.id}
          tagId={updateModalCategory.id}
          onClose={updateModalClose}
          defaultValues={{
            title: updateModalCategory.title,
            type: updateModalCategory.type,
            showOnMainPage: updateModalCategory.showOnMainPage,
          }}
          onSuccess={onUpdateSuccess}
        />
      ) : undefined}
    </PageLayout>
  );
};

export default AdminTags;
