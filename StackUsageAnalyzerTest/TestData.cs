namespace StackUsageAnalyzerTest
{
    static class TestData
    {
        public static string osc8_calib = @"tc.h:1142:20:tc_enable_events	8	static
osc8_calib.c:243:17:get_osc_frequency	24	dynamic,bounded
usart_serial.h:106:20:usart_serial_getchar	24	static
usart_serial.h:92:32:usart_serial_putchar	16	static
osc8_calib.c:263:5:main	144	static
";

        public static string interrupt_sam_nvic = @"interrupt_sam_nvic.c:57:6:cpu_irq_enter_critical	0	static
interrupt_sam_nvic.c:73:6:cpu_irq_leave_critical	0	static
";

        public static string systick_counter = @"systick_counter.c:61:6:delay_init	8	static
systick_counter.c:75:6:delay_cycles_us	12	static
systick_counter.c:89:6:delay_cycles_ms	12	static
";

        public static string board_init = @"board_init.c:59:6:system_board_init	32	static
";

        public static string sercom_interrupt = @"sercom_interrupt.c:62:13:_sercom_default_handler	0	static
sercom_interrupt.c:75:6:_sercom_set_handler	20	static
sercom_interrupt.c:125:30:_sercom_get_interrupt_vector	16	static
sercom_interrupt.c:141:1:SERCOM0_Handler	8	static
sercom_interrupt.c:141:1:SERCOM1_Handler	8	dynamic
sercom_interrupt.c:141:1:SERCOM2_Handler	8	static
sercom_interrupt.c:141:1:SERCOM3_Handler	8	static
sercom_interrupt.c:141:1:SERCOM4_Handler	8	static
sercom_interrupt.c:141:1:SERCOM5_Handler	8	static
";

        public static string events = @"events.c:70:10:_events_find_bit_position	0	static
events.c:134:6:_system_events_init	0	static
events.c:149:6:events_get_config_defaults	0	static
events.c:160:18:events_allocate	32	static
events.c:218:18:events_trigger	8	static
events.c:252:6:events_is_busy	8	static
events.c:198:18:events_release	8	static
events.c:260:6:events_is_users_ready	8	static
events.c:268:6:events_is_detected	8	static
events.c:284:6:events_is_overrun	8	static
events.c:300:18:events_attach_user	0	static
events.c:314:18:events_detach_user	0	static
events.c:325:9:events_get_free_channels	0	static
";
    }
}
