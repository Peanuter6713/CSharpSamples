using Stateless;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace XieCheng.Models
{
    /// <summary>
    /// 对应订单有限状态的第一列(现态)
    /// </summary>
    public enum OrderStateEnum
    {
        /// <summary>
        /// 订单已生成
        /// </summary>
        Pending,
        /// <summary>
        /// 支付处理中
        /// </summary>
        Processing,
        /// <summary>
        /// 交易完成
        /// </summary>
        Completed,
        /// <summary>
        /// 交易失败
        /// </summary>
        Declined,
        /// <summary>
        /// 订单已取消
        /// </summary>
        Cancelled,
        /// <summary>
        /// 已退款
        /// </summary>
        Refund    
    }

    /// <summary>
    /// 对应订单有限状态的第3列(动作)
    /// </summary>
    public enum OrderStateTriggerEnum
    {
        /// <summary>
        /// 支付
        /// </summary>
        PlaceOrder,
        /// <summary>
        /// 支付成功
        /// </summary>
        Approve,
        /// <summary>
        /// 支付失败
        /// </summary>
        Reject,
        /// <summary>
        /// 取消订单
        /// </summary>
        Cancel,    
        /// <summary>
        /// 退货
        /// </summary>
        Return      
    }

    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }  // 订单所属用户
        public ApplicationUser User { get; set; } // foreignkey
        public ICollection<LineItem> OrderItems { get; set; } // 订单的商品信息
        public OrderStateEnum State { get; set; } // 订单状态
        public DateTime CreateDateUTC { get; set; }
        public string TransactionMetadata { get; set; } // 第三方交易的数据

        StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine;

        public Order()
        {
            StateMachineInit();
        }


        public void PaymentProcessing()
        {
            try
            {
                _machine.Fire(OrderStateTriggerEnum.PlaceOrder);
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public void PaymentApprove()
        {
            _machine.Fire(OrderStateTriggerEnum.Approve);
        }

        public void PaymentReject()
        {
            _machine.Fire(OrderStateTriggerEnum.Reject);
        }

        private void StateMachineInit()
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(
                () => State,
                s => State = s
            );

            //_machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>
            //    (OrderStateEnum.Pending);

            _machine.Configure(OrderStateEnum.Pending)
                // 如果传入的状态为支付，则下一个动作为 ‘支付处理中’
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            _machine.Configure(OrderStateEnum.Declined)
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing);

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);
        }


    }
}
