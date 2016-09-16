declare @actionItemId int = (select id from dbo.ActionItems where method_name = 'CancelLastSavingEvent')
delete from dbo.AllowedRoleActions where action_item_id = @actionItemId
delete from dbo.ActionItems where method_name = 'CancelLastSavingEvent'

insert into dbo.ActionItems(class_name, method_name)
values('SavingServices', 'CancelLastSavingEvent')
GO

declare @actionItemId int = (select id from dbo.ActionItems where method_name = 'CancelLastSavingEvent')
if not @actionItemId is null
begin
    delete from dbo.AllowedRoleActions where action_item_id = @actionItemId

    insert into dbo.AllowedRoleActions (action_item_id, role_id, allowed)
    select
        @actionItemId
        , id
        , case
            when code in ('ADMIN', 'SUPER', 'IT', 'ACCOUNTING') then 1
            else 0
        end
    from
        dbo.Roles
end
GO

update
    dbo.TechnicalParameters
set
    value = 'v16.9.0.0'
where
    name = 'VERSION'
GO