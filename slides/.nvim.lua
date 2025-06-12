local lspconfig = require("lspconfig")

lspconfig.texlab.setup({})

vim.g.vimtex_compiler_engine = 'lualatex'
vim.g.vimtex_quickfix_enabled = 0
